using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Numerics;

namespace Primitive.Shapes
{
	internal class Ellipse : IShape
	{
		private const float MinSize = .01f;
		private const float DefaultAlpha = .5f;
        private readonly static Random Rand = new();

        public Vector4 Color { get; private set; }

        public Vector2 Position { get; private set; }

		public Vector2 Size { get; private set; }

		public Ellipse()
		{
            Position = Rand.NextVector2();
            Size = Helper.Clamp(Rand.NextVector2(), MinSize, 1);
        }

		public IShape Clone() => (IShape)MemberwiseClone();

		public void Randomize()
		{
			Position = Rand.NextVector2();
			Size = Helper.Clamp(Rand.NextVector2(), MinSize, 1);
		}

		public void Mutate()
		{
			switch (Rand.Next(3))
			{
				case 0:
					Position += Rand.NextVector2Signed() / 16;
					Position = Helper.Clamp(Position, 0, 1);
					break;
				case 1:
					Size += Rand.NextVector2Signed() / 16;
					Size = Helper.Clamp(Size, MinSize, 1);
					break;
				case 2:
					Color += Rand.NextVector4Signed() / 16;
					Color = Helper.Clamp(Color, 0, 1);
					break;
			}
		}

		public void Sample(Image<Rgba32> image)
		{
			var area = Bounds(image.Bounds);
			Color = (Vector4)Helper.AverageColor(image, area).WithAlpha(DefaultAlpha);
		}

		public void Draw(Image<Rgba32> image)
		{
			var (position, size) = (Position * image.Width, Size * image.Width);
			var path = new EllipsePolygon(position, new SizeF(size));
			image.Mutate(x => x.Fill(new Color(Color), path));
		}

		public Rectangle Bounds(Rectangle area)
		{
			var (position, size) = (Position * area.Width, Size * area.Width);
			return new(Point.Round(position - size / 2), new(Point.Round(size)));
		}
	}
}
