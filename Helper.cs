using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.Numerics;

namespace Primitive
{
	internal static class Helper
	{
		#region Image

		public static Color AverageColor(Image<Rgba32> image, Rectangle? area = null)
		{
			var rect = area ?? image.Bounds;

			var a = Math.Max(0, rect.Top);
			var b = Math.Min(image.Height, rect.Bottom);
			var c = Math.Max(0, rect.Left);
			var d = Math.Min(image.Width, rect.Right);

			var sum = Vector4.Zero;
			var pixels = (b - a) * (d - c);

			image.ProcessPixelRows(rows =>
			{
				for (var i = a; i < b; i++)
				{
					var row = rows.GetRowSpan(i);
					for (var j = c; j < d; j++)
					{
						sum += row[j].ToVector4();
					}
				}
			});

			return new Color(sum / pixels);
		}

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

			return (float)Math.Sqrt(
				(sum.W + sum.X + sum.Y + sum.Z) / channels);
		}

		#endregion

		#region Vector

		public static Vector2 NextVector2(this Random rand)
		{
			return new(
				rand.NextSingle(),
				rand.NextSingle()
			);
		}

		public static Vector2 NextVector2Signed(this Random rand)
		{
			return new(
				rand.NextSingle() * rand.NextSign(),
				rand.NextSingle() * rand.NextSign()
			);
		}

		public static Vector4 NextVector4(this Random rand)
		{
			return new(
				rand.NextSingle(),
				rand.NextSingle(),
				rand.NextSingle(),
				rand.NextSingle()
			);
		}

		public static Vector4 NextVector4Signed(this Random rand)
		{
			return new(
				rand.NextSingle() * rand.NextSign(),
				rand.NextSingle() * rand.NextSign(),
				rand.NextSingle() * rand.NextSign(),
				rand.NextSingle() * rand.NextSign()
			);
		}

		public static int NextSign(this Random rand)
			=> rand.Next(2) * 2 - 1;

		public static Vector2 Clamp(Vector2 val, float min, float max)
		{
			return new(
				Math.Clamp(val.X, min, max),
				Math.Clamp(val.Y, min, max)
			);
		}

		public static Vector4 Clamp(Vector4 val, float min, float max)
		{
			return new(
				Math.Clamp(val.X, min, max),
				Math.Clamp(val.Y, min, max),
				Math.Clamp(val.Z, min, max),
				Math.Clamp(val.W, min, max)
			);
		}

		#endregion
	}
}
