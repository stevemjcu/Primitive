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
		private static readonly Random _rand = new();

		public override Shape New()
		{
			return new Ellipse()
			{
				Position = _rand.NextVector2(),
				Size = _rand.NextVector2() + new Vector2(.02f)
			};
		}

		public override void Mutate()
		{
			switch (_rand.Next(3))
			{
				case 0:
					Position = Helper.Clamp(Position + _rand.NextSignedVector2() / 16, 0, 1);
					break;
				case 1:
					Size = Helper.Clamp(Size + _rand.NextSignedVector2() / 16, .02f, 1);
					break;
				case 2:
					Color = Helper.Clamp(Color + _rand.NextSignedVector4() / 16, 0, 1);
					break;
			}
		}

		public override void Sample(Image<Rgba32> image)
		{
			var (position, size) = (Position * image.Width, Size * image.Width);
			var area = new Rectangle(Point.Round(position - size / 2), new(Point.Round(size)));
			Color = (Vector4)Helper.AverageColor(image, area).WithAlpha(.5f);
		}

		public override void Draw(Image<Rgba32> image)
		{
			var (position, size) = (Position * image.Width, Size * image.Width);
			var path = new EllipsePolygon(position, new SizeF(size));
			image.Mutate(x => x.Fill(new Color(Color), path));
		}
	}
}
