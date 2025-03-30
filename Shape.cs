using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.Numerics;

namespace Primitive
{
	internal abstract class Shape
	{
		protected readonly static Random Rand = new();

		public Vector4 Color { get; protected set; }

		public Shape Clone() => (Shape)MemberwiseClone();

		public abstract void Randomize();

		public abstract void Mutate();

		public abstract void Sample(Image<Rgba32> image);

		public abstract void Draw(Image<Rgba32> image);

		public abstract Rectangle Bounds(Rectangle area);
	}
}
