using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.Numerics;

namespace Primitive
{
	/// <summary>
	/// A class that represents a mutable shape on a bounded plane.
	/// </summary>
	/// <remarks>Each axis should be constrained to [0.0, 1.0)</remarks>
	internal abstract class Shape
	{
		public Vector2 Position { get; protected set; }

		public Vector2 Size { get; protected set; }

		public Vector4 Color { get; protected set; }

		public float Error { get; set; } = int.MaxValue;

		/// <summary>
		/// Creates a random <see cref="Shape"/> of the same type.
		/// </summary>
		/// <returns>The new <see cref="Shape"/></returns>
		public abstract Shape New();

		/// <summary>
		/// Clones the current <see cref="Shape"/>.
		/// </summary>
		/// <returns>The new <see cref="Shape"/></returns>
		public Shape Clone() => (Shape)MemberwiseClone();

		/// <summary>
		/// Randomly modifies one property of the current <see cref="Shape"/>.
		/// </summary>
		public abstract void Mutate();

		/// <summary>
		/// Evaluates the <see cref="Color"/> property by sampling the given <see cref="Image{Rgba32}"/>.
		/// </summary>
		/// <param name="image">The <see cref="Image{Rgba32}"/> to sample from</param>
		public abstract void Sample(Image<Rgba32> image);

		/// <summary>
		/// Draws the current <see cref="Shape"/> onto the given <see cref="Image{Rgba32}"/>.
		/// </summary>
		/// <param name="image">The <see cref="Image{Rgba32}"/> to draw on</param>
		public abstract void Draw(Image<Rgba32> image);
	}
}
