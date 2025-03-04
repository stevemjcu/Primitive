using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Primitive
{
	internal class Model<T>(Image<Rgba32> target, Color background) where T : IShape
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
		public Queue<T> Shapes { get; } = [];

		/// <summary>
		/// The root mean square error (RMSE) between the <see cref="Current"/> and <see cref="Target"/> images.
		/// </summary>
		public int Score { get; set; }

		/// <summary>
		/// Adds a random <see cref="{T}"/> to the <see cref="Current"/> image using a hill climbing algorithm.
		/// </summary>
		public void Add()
		{

		}

		/// <summary>
		/// Recreates the <see cref="Current"/> image at the given resolution with anti-aliasing.
		/// </summary>
		/// <param name="size"></param>
		public Image<Rgba32> Export(int size)
		{
			return Current;
		}
	}
}
