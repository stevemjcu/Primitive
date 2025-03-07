using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.Numerics;

namespace Primitive
{
	/// <summary>
	/// A class that represents a mutable shape on a bounded grid.
	/// Each axis is constrained to [0.0, 1.0).
	/// </summary>
	internal abstract class Shape
	{
		public Vector2 Position { get; private set; }

		public Vector2 Size { get; private set; }

		public Vector4 Color { get; private set; }

		public int Error { get; set; } = int.MaxValue;

		/// <summary>
		/// Creates a new <see cref="Shape"/> of the same type.
		/// </summary>
		/// <returns>The new shape</returns>
		public abstract Shape New();

		/// <summary>
		/// Clones the current <see cref="Shape"/>.
		/// </summary>
		/// <returns>The new shape</returns>
		public Shape Clone() => (Shape)MemberwiseClone();

		/// <summary>
		/// Randomizes the properties of the current <see cref="Shape"/>.
		/// </summary>
		public abstract void Randomize();

		/// <summary>
		/// Randomly modifies one property of the current <see cref="Shape"/>.
		/// </summary>
		public abstract void Mutate();

		/// <summary>
		/// Updates the <see cref="Color"/> property by sampling the given <see cref="Image{Rgba32}"/>.
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
