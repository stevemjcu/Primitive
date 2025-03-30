using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Primitive
{
	internal class Model
	{
		public Image<Rgba32> Target { get; }

		public Image<Rgba32> Current { get; private set; }

		private float Error { get; set; }

		private Queue<Shape> Shapes { get; } = [];

		private Color Background { get; }

		public Model(Image<Rgba32> target) : this(target, Helper.AverageColor(target)) { }

		public Model(Image<Rgba32> target, Color background)
		{
			Target = target;
			Current = new(Target.Width, Target.Height, background);
			Error = Helper.RootMeanSquareError(Current, target);
			Background = background;
		}

		public void Add<T>(int trials, int failures) where T : Shape, new()
		{
			var state = Optimize(Trial<T>(trials), failures);
			Current = state.Image;
			Shapes.Enqueue(state.Shape);
		}

		private State Trial<T>(int n) where T : Shape, new()
		{
			var best = new State(new T(), Current);
			for (var i = 0; i < n; i++)
			{
				var shape = new T();
				var image = Current.Clone();

				shape.Randomize();
				shape.Sample(Target);
				shape.Draw(image);

				var error = Helper.RootMeanSquareError(
					Current, image, Target, shape.Bounds(image.Bounds), Error);
				if (error < best.Error)
					best = new(shape, image, error);
			}
			return best;
		}

		private State Optimize(State start, int n)
		{
			var best = start;
			for (var i = 0; i < n; i++)
			{
				var shape = best.Shape.Clone();
				var image = Current.Clone();

				shape.Mutate();
				shape.Draw(image);

				var error = Helper.RootMeanSquareError(
					best.Image, image, Target, shape.Bounds(image.Bounds), best.Error);
				if (error < best.Error)
					(best, i) = (new(shape, image, error), 0);
			}
			return best;
		}

		public Image<Rgba32> Export(int size)
		{
			var image = new Image<Rgba32>(size, size, Background);
			foreach (var s in Shapes) s.Draw(image);
			return image;
		}
	}

	internal struct State(Shape shape, Image<Rgba32> image, float error = float.MaxValue)
	{
		public Shape Shape = shape;
		public Image<Rgba32> Image = image;
		public float Error = error;
	}
}
