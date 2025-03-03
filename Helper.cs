using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.Numerics;

namespace Primitive
{
	internal static class Helper
	{
		/// <summary>
		/// Calculates the average <see cref="Color"/> of the given <see cref="Image{Rgba32}"/>.
		/// </summary>
		/// <param name="image">The target <see cref="Image{Rgba32}"/>.</param>
		/// <returns>The average <see cref="Color"/>.</returns>
		public static Color AverageColor(Image<Rgba32> image)
		{
			var sum = new Vector4();

			image.ProcessPixelRows(rows =>
			{
				for (var i = 0; i < rows.Height; i++)
					foreach (var p in rows.GetRowSpan(i))
						sum += p.ToVector4();
			});

			return new Color(sum / (image.Height * image.Width));
		}

		/// <summary>
		/// Calculates the root mean squared error between an <see cref="Image{Rgba32}"/> and its target.
		/// </summary>
		/// <param name="source">The model <see cref="Image{Rgba32}"/>.</param>
		/// <param name="target">The ideal <see cref="Image{Rgba32}"/>.</param>
		/// <returns>The root mean squared error.</returns>
		public static int RMSE(Image<Rgba32> source, Image<Rgba32> target)
		{
			return default;
		}
	}
}
