using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Primitive
{
	internal class Model
	{
		/// <summary>
		/// The thumbnail image corresponding to the model's goal.
		/// </summary>
		public Image<Rgba32> Target { get; }

		/// <summary>
		/// The thumbnail image corresponding to the model's canvas.
		/// </summary>
		public Image<Rgba32> Current { get; }

		/// <summary>
		/// The list of shapes which compose the <see cref="Current"/> image.
		/// </summary>
		public Queue<Shape> Shapes { get; } = [];

		public Model(Image<Rgba32> target, Color background)
		{
			Target = target;
			Current = new(Target.Width, Target.Height, background);
		}

		/// <summary>
		/// Adds the <see cref="Shape"/> to the <see cref="Current"/> image using a hill climbing algorithm.
		/// </summary>
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

		/// <summary>
		/// Redraws the <see cref="Current"/> image using the <see cref="Shapes"/> queue at the given resolution.
		/// </summary>
		public Image<Rgba32> Export(int size, Color background)
		{
			// TODO: Investigate options like anti-aliasing
			var image = new Image<Rgba32>(size, size, background);
			foreach (var s in Shapes) s.Draw(image);
			return image;
		}
	}
}
