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
		/// Assumes images have the same dimensions.
		/// </summary>
		/// <param name="current">The model <see cref="Image{Rgba32}"/>.</param>
		/// <param name="target">The ideal <see cref="Image{Rgba32}"/>.</param>
		/// <returns>The average distance between corresponding color channels.</returns>
		public static int Rmse(Image<Rgba32> current, Image<Rgba32> target)
		{
			var sum = Vector4.Zero;
			var channels = current.Height * current.Width * 4;
			current.ProcessPixelRows(target, (rows1, rows2) =>
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

		public static Vector2 NextVector2(this Random rand)
		{
			return new()
			{
				X = rand.NextSingle(),
				Y = rand.NextSingle()
			};
		}

		public static Vector4 NextVector4(this Random rand)
		{
			return new()
			{
				W = rand.NextSingle(),
				X = rand.NextSingle(),
				Y = rand.NextSingle(),
				Z = rand.NextSingle()
			};
		}

		public static Vector2 Clamp(Vector2 val, float min, float max)
		{
			return new()
			{
				X = Math.Clamp(val.X, min, max),
				Y = Math.Clamp(val.Y, min, max)
			};
		}

		public static Vector4 Clamp(Vector4 val, float min, float max)
		{
			return new()
			{
				W = Math.Clamp(val.W, min, max),
				X = Math.Clamp(val.X, min, max),
				Y = Math.Clamp(val.Y, min, max),
				Z = Math.Clamp(val.Z, min, max),
			};
		}

		public static int NextSign(this Random rand)
			=> rand.Next(2) == 0 ? 1 : -1;
	}
}
