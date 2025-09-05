using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Primitive
{
	internal interface IShape
	{
		public IShape Clone();

		public void Mutate();

		public void Sample(Image<Rgba32> image);

		public void Draw(Image<Rgba32> image);

		public Rectangle Bounds(Rectangle area);
	}
}
