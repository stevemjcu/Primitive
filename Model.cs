using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Primitive
{
	internal class Model
	{
		public Image<Rgba32> Target { get; }

		public Image<Rgba32> Current { get; }

		public Queue<Shape> Shapes { get; } = [];

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
			Shapes.Enqueue(shape);
		}

		private Shape Trial<T>(int n) where T : Shape, new()
		{
			var shape = new T();
			for (var i = 0; i < n; i++)
			{
				var c = Current.Clone();
				var s = new T();

				s.Randomize();
				s.Sample(Target);
				s.Draw(c);

				s.Error = Helper.Rmse(c, Target);
				if (s.Error < shape.Error)
					shape = s;
			}
			return shape;
		}

		private Shape Optimize(Shape shape, int n)
		{
			for (var i = 0; i < n; i++)
			{
				var c = Current.Clone();
				var s = shape.Clone();

				s.Mutate();
				s.Draw(c);

				s.Error = Helper.Rmse(c, Target);
				if (s.Error < shape.Error)
					(shape, i) = (s, 0);
			}
			return shape;
		}

		public Image<Rgba32> Export(int size)
		{
			var image = new Image<Rgba32>(size, size, Background);
			foreach (var s in Shapes) s.Draw(image);
			return image;
		}
	}
}
