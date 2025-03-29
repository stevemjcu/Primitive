using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Primitive
{
	internal class Model
	{
		private Image<Rgba32> Target { get; }

		private Image<Rgba32> Current { get; }

		private float Error { get; set; }

		private Queue<Shape> Shapes { get; } = [];

		private Color Background { get; }

		public Model(Image<Rgba32> target) : this(target, Helper.AverageColor(target)) { }

		public Model(Image<Rgba32> target, Color background)
		{
			Target = target;
			Current = new(Target.Width, Target.Height, background);
			Background = background;
		}

		public void Add<T>(int trials, int failures) where T : Shape, new()
		{
			var shape = Optimize(Trial<T>(trials), failures);
			shape.Draw(Current);
			Error = Helper.AverageError(Current, Target);
			Shapes.Enqueue(shape);
		}

		private Shape Trial<T>(int n) where T : Shape, new()
		{
			var best = new T();
			for (var i = 0; i < n; i++)
			{
				var shape = new T();
				var image = Current.Clone();

				shape.Randomize();
				shape.Sample(Target);
				shape.Draw(image);

				shape.Error = Helper.AverageError(image, Target);
				if (shape.Error < best.Error) best = shape;
			}
			return best;
		}

		private Shape Optimize(Shape start, int n)
		{
			var best = start;
			for (var i = 0; i < n; i++)
			{
				var shape = best.Clone();
				var image = Current.Clone();

				shape.Mutate();
				shape.Draw(image);

				shape.Error = Helper.AverageError(image, Target);
				if (shape.Error < best.Error) (best, i) = (shape, 0);
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
}
