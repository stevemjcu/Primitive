using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Primitive.Shapes
{
	class Ellipse : Shape
	{
		public override void Draw(Image<Rgba32> image)
		{
			var shape = new EllipsePolygon(Position, new SizeF(Size));
			image.Mutate(x => x.Fill(Color, shape));
		}
	}
}
