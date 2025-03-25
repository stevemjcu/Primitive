using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Primitive
{
	internal class Model
	{
		public Image<Rgba32> Target { get; }

		public Image<Rgba32> Current { get; }

		public Queue<Shape> Shapes { get; } = [];

		public Model(Image<Rgba32> target, Color background)
		{
			Target = target;
			Current = new(Target.Width, Target.Height, background);
		}

		public void Add(Shape shape, int trials, int limit)
		{
			var s = Optimize(Trial(shape, trials), limit);
			s.Draw(Current);
			Shapes.Enqueue(s);
		}

		private Shape Trial(Shape shape, int trials)
		{
			for (var i = 0; i < trials; i++)
			{
				var c = Current.Clone();
				var s = shape.New();

				s.Sample(Target);
				s.Draw(c);

				s.Error = Helper.Rmse(c, Target);
				if (s.Error < shape.Error)
					shape = s;
			}
			return shape;
		}

		private Shape Optimize(Shape shape, int limit)
		{
			for (var i = 0; i < limit; i++)
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

		public Image<Rgba32> Export(int size, Color background)
		{
			var image = new Image<Rgba32>(size, size, background);
			foreach (var s in Shapes) s.Draw(image);
			return image;
		}
	}
}
