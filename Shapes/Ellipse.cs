using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Primitive.Shapes
{
	class Ellipse : Shape
	{
		public override void Randomize()
		{
			throw new NotImplementedException();
		}

		public override void Mutate()
		{
			throw new NotImplementedException();
		}

		public override void Sample(Image<Rgba32> image)
		{
			throw new NotImplementedException();
		}

		public override void Draw(Image<Rgba32> image)
		{
			var path = new EllipsePolygon(Position * image.Width, new SizeF(Size * image.Width));
			image.Mutate(x => x.Fill(new Color(Color), path));
		}
	}
}
