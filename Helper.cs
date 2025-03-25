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
		public static Color AverageColor(Image<Rgba32> image, Rectangle? area = null)
		{
			var rect = area ?? image.Bounds;
			var i1 = Math.Max(0, rect.Top);
			var i2 = Math.Min(image.Height, rect.Bottom);
			var j1 = Math.Max(0, rect.Left);
			var j2 = Math.Min(image.Width, rect.Right);

			var sum = Vector4.Zero;
			var pixels = (i2 - i1) * (j2 - j1);
			image.ProcessPixelRows(rows =>
			{
				for (var i = i1; i < i2; i++)
				{
					var row = rows.GetRowSpan(i);
					for (var j = j1; j < j2; j++)
					{
						sum += row[j].ToVector4();
					}
				}
			});
			return new Color(sum / pixels);
		}

		/// <summary>
		/// Calculates the root mean squared error between the source <see cref="Image{Rgba32}"/> and its target.
		/// </summary>
		/// <param name="source">The model <see cref="Image{Rgba32}"/>.</param>
		/// <param name="target">The ideal <see cref="Image{Rgba32}"/>.</param>
		/// <returns>The average distance between corresponding color channels</returns>
		/// <remarks>Assumes images have the same dimensions</remarks>
		public static float Rmse(Image<Rgba32> source, Image<Rgba32> target)
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
			return (float)Math.Sqrt((sum.W + sum.X + sum.Y + sum.Z) / channels);
		}

		public static Vector2 NextVector2(this Random rand)
		{
			return new()
			{
				X = rand.NextSingle(),
				Y = rand.NextSingle()
			};
		}

		public static Vector2 NextSignedVector2(this Random rand)
		{
			return new()
			{
				X = rand.NextSingle() * rand.NextSign(),
				Y = rand.NextSingle() * rand.NextSign()
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

		public static Vector4 NextSignedVector4(this Random rand)
		{
			return new()
			{
				W = rand.NextSingle() * rand.NextSign(),
				X = rand.NextSingle() * rand.NextSign(),
				Y = rand.NextSingle() * rand.NextSign(),
				Z = rand.NextSingle() * rand.NextSign()
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
