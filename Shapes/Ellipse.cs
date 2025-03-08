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
				Size = _rand.NextVector2() / 3 + new Vector2(0.1f)
			};
		}

		public override void Mutate()
		{
			Position = Helper.Clamp(Position + _rand.NextVector2() * _rand.NextSign(), 0, 1);
			Size = Helper.Clamp(Size + _rand.NextVector2() * _rand.NextSign(), 0, 0.5f);
		}

		public override void Sample(Image<Rgba32> image)
		{
			// TODO: This should sample the local area
			Color = _rand.NextVector4();
		}

		public override void Draw(Image<Rgba32> image)
		{
			var path = new EllipsePolygon(Position * image.Width, new SizeF(Size * image.Width));
			image.Mutate(x => x.Fill(new Color(Color), path));
		}

		public override void Score(Image<Rgba32> current, Image<Rgba32> target)
		{
			// TODO: This should diff the local area
			Error = Helper.Rmse(current, target);
		}
	}
}
