using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.Numerics;

namespace Primitive
{
	internal abstract class Shape
	{
		public Vector2 Position { get; set; }

		public Vector4 Color { get; set; }

		public int Error { get; set; } = int.MaxValue;

		public Shape Clone() => (Shape)MemberwiseClone();

		public abstract void Randomize();

		public abstract void Mutate();

		public abstract Rgba32 Sample(Image<Rgba32> image);

		public abstract void Draw(Image<Rgba32> image);
	}
}
