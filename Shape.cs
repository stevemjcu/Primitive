using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.Numerics;

namespace Primitive
{
	internal abstract class Shape
	{
		public Vector2 Position { get; set; }

		public Vector2 Size { get; set; }

		public float Rotation { get; set; }

		public Rgba32 Color { get; set; }

		public abstract void Draw(Image<Rgba32> image);
	}
}
