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
			var sum = Vector4.Zero;
			var pixels = image.Height * image.Width;
			image.ProcessPixelRows(rows =>
			{
				for (var i = 0; i < rows.Height; i++)
					foreach (var p in rows.GetRowSpan(i))
						sum += p.ToVector4();
			});
			return new Color(sum / pixels);
		}

		/// <summary>
		/// Calculates the root mean squared error between an <see cref="Image{Rgba32}"/> and its target.
		/// Assumes images are the same dimensions.
		/// </summary>
		/// <param name="source">The model <see cref="Image{Rgba32}"/>.</param>
		/// <param name="target">The ideal <see cref="Image{Rgba32}"/>.</param>
		/// <returns>The average distance between corresponding color channels.</returns>
		public static int RMSE(Image<Rgba32> source, Image<Rgba32> target)
		{
			var sum = Vector4.Zero;
			var channels = source.Height * source.Width * 4;
			source.ProcessPixelRows(target, (rows1, rows2) =>
			{
				for (var i = 0; i < rows1.Height; i++)
				{
					var row1 = rows1.GetRowSpan(i);
					var row2 = rows2.GetRowSpan(i);
					for (var j = 0; j < rows1.Width; j++)
					{
						var diff = row1[j].ToVector4() - row2[j].ToVector4();
						sum += diff * diff;
					}
				}
			});
			return (int)Math.Sqrt((sum.W + sum.X + sum.Y + sum.Z) / channels);
		}
	}
}
