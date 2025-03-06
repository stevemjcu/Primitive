using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Numerics;

namespace Primitive.Shapes
{
	class Ellipse : Shape
	{
		public Vector2 Size { get; set; }

		public float Rotation { get; set; }

		public override void Randomize()
		{
			throw new NotImplementedException();

			//new Shape()
			//{
			//	Position = new()
			//	Size = Rand.NextVector2(Length / 16, Length / 4),
			//	Color = Color.Black, // TODO: Pick color
			//};
		}

		public override void Mutate()
		{
			throw new NotImplementedException();

			//switch (Rand.Next(3))
			//{
			//	case 0:
			//		// TODO: Mutate position
			//		break;
			//	case 1:
			//		// TODO: Mutate size
			//		break;
			//	case 2:
			//		// TODO: Mutate color
			//		break;
			//}
		}

		public override Rgba32 Sample(Image<Rgba32> image)
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
