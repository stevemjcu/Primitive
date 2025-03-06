using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Primitive
{
	internal class Model(Image<Rgba32> target, Color background)
	{
		/// <summary>
		/// The thumbnail image corresponding to the model's goal.
		/// </summary>
		public Image<Rgba32> Target { get; } = target;

		/// <summary>
		/// The thumbnail image corresponding to the model's canvas.
		/// </summary>
		public Image<Rgba32> Current { get; } = new(target.Width, target.Height, background);

		/// <summary>
		/// The list of shapes which compose the <see cref="Current"/> image.
		/// </summary>
		public Queue<Shape> Shapes { get; } = [];

		private readonly Color _background = background;

		/// <summary>
		/// Adds a random <see cref="{T}"/> shape to the <see cref="Current"/> image using a hill climbing algorithm.
		/// </summary>
		public void AddShape<T>(int trials, int limit) where T : Shape, new()
		{
			var s = OptimizeShape<T>(TrialShapes<T>(trials), limit);
			Shapes.Enqueue(s);
			s.Draw(Current);
		}

		private Shape TrialShapes<T>(int trials) where T : Shape, new()
		{
			var shape = new T();
			for (var i = 0; i < trials; i++)
			{
				var c = Current.Clone();
				var s = new T();

				s.Randomize();
				s.Sample(Target);
				s.Draw(c);

				s.Error = Helper.RMSE(c, Target);
				if (s.Error < shape.Error)
					shape = s;
			}
			return shape;
		}

		private Shape OptimizeShape<T>(Shape shape, int limit) where T : Shape, new()
		{
			for (var i = 0; i < limit; i++)
			{
				var c = Current.Clone();
				var s = (T)shape.Clone();

				s.Mutate();
				s.Draw(c);

				s.Error = Helper.RMSE(c, Target);
				if (s.Error < shape.Error)
					(shape, i) = (s, 0);
			}
			return shape;
		}

		/// <summary>
		/// Redraws the <see cref="Current"/> image using the <see cref="Shapes"/> queue at the given resolution.
		/// </summary>
		public Image<Rgba32> Export(int length)
		{
			// TODO: Enable options like anti-aliasing
			var image = new Image<Rgba32>(length, length, _background);
			foreach (var s in Shapes) s.Draw(image);
			return image;
		}
	}
}
