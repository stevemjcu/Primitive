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
		public Image<Rgba32> Current { get; } = new Image<Rgba32>(target.Width, target.Height, background);

		/// <summary>
		/// The list of shapes which compose the <see cref="Current"/> image.
		/// </summary>
		public Queue<Shape> Shapes { get; } = [];

		private Random Rand { get; } = new Random();

		private int Length { get; } = target.Width;

		/// <summary>
		/// Adds a random <see cref="{T}"/> shape to the <see cref="Current"/> image using a hill climbing algorithm.
		/// </summary>
		public void AddShape<T>(int trials, int limit) where T : Shape, new()
		{
			var shape = new T();
			var error = int.MaxValue;

			// Trial series of shapes and pick best
			for (var i = 0; i < trials; i++)
			{
				var c = Current.Clone();
				var s = new T()
				{
					Position = Rand.NextVector2(Length),
					Size = Rand.NextVector2(Length / 16, Length / 4),
					Color = Color.Black, // TODO: Pick color
				};

				// Should we sample color from a region e.g., average or median or random?
				// Or should we precalculate and unroll a color palette to choose from?
				// Or is it only feasible to recalculate color before every draw call?

				s.Draw(c);
				var e = Helper.RMSE(c, Target);
				if (e < error)
					(shape, error) = (s, e);
			}

			// Optimize shape until it ages out
			for (var i = 0; i < limit; i++)
			{
				var c = Current.Clone();
				var s = (T)shape.Clone();

				switch (Rand.Next(3))
				{
					case 0:
						// TODO: Mutate position
						break;
					case 1:
						// TODO: Mutate size
						break;
					case 2:
						// TODO: Mutate color
						break;
				}

				s.Draw(c);
				var e = Helper.RMSE(c, Target);
				if (e < error)
					(shape, error, i) = (s, e, 0);
			}
		}

		/// <summary>
		/// Redraws the <see cref="Current"/> image at the given resolution.
		/// Also enables anti-aliasing.
		/// </summary>
		public Image<Rgba32> Export(int length)
		{
			return Current;
		}
	}
}
