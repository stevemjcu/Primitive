using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Primitive
{
	internal interface IShape
	{
		/// <summary>
		/// Clones the shape.
		/// </summary>
		/// <returns></returns>
		public IShape Clone();

		/// <summary>
		/// Randomly modifies the shape.
		/// </summary>
		public void Mutate();

		/// <summary>
		/// Determines the shape's color from the image.
		/// </summary>
		/// <param name="image"></param>
		public void Sample(Image<Rgba32> image);

		/// <summary>
		/// Draws the shape on the image.
		/// </summary>
		/// <param name="image"></param>
		public void Draw(Image<Rgba32> image);

		/// <summary>
		/// The axis-aligned bounds of the shape.
		/// </summary>
		public Rectangle Bounds(Rectangle area);
	}
}
