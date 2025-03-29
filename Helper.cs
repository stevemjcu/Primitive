using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.Numerics;

namespace Primitive
{
	internal static class Helper
	{
		#region Image

		public static Color AverageColor(Image<Rgba32> image)
			=> AverageColor(image, image.Bounds);

		public static Color AverageColor(Image<Rgba32> image, Rectangle area)
		{
			area = Rectangle.Intersect(image.Bounds, area);
			var sum = Vector4.Zero;
			var pixels = area.Width * area.Height;

			image.ProcessPixelRows(rows =>
			{
				for (var i = area.Top; i < area.Bottom; i++)
				{
					var row = rows.GetRowSpan(i);
					for (var j = area.Left; j < area.Height; j++)
					{
						sum += row[j].ToVector4();
					}
				}
			});

			return new Color(sum / pixels);
		}

		public static float AverageError(Image<Rgba32> source, Image<Rgba32> target)
		{
			var sum = 0f;
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
						sum += (diff * diff).Sum();
					}
				}
			});

			return (float)Math.Sqrt(sum / channels);
		}

		// Calculate diff in area both before and after
		// Subtract previous diff from total, replace with new diff
		// Return total
		public static float AverageError(Image<Rgba32> before, Image<Rgba32> after, Image<Rgba32> target, Rectangle area, float error)
		{
			area = Rectangle.Intersect(before.Bounds, area);
			var channels = before.Height * before.Width * 4;
			var sum = (float)Math.Pow(error, 2) * channels;

			before.ProcessPixelRows(target, (rows1, rows2) =>
			{
				for (var i = area.Top; i < area.Bottom; i++)
				{
					var row1 = rows1.GetRowSpan(i);
					var row2 = rows2.GetRowSpan(i);
					for (var j = area.Left; j < area.Right; j++)
					{
						var diff = row1[j].ToVector4() - row2[j].ToVector4();
						sum -= (diff * diff).Sum();
					}
				}
			});

			after.ProcessPixelRows(target, (rows1, rows2) =>
			{
				for (var i = area.Top; i < area.Bottom; i++)
				{
					var row1 = rows1.GetRowSpan(i);
					var row2 = rows2.GetRowSpan(i);
					for (var j = area.Left; j < area.Right; j++)
					{
						var diff = row1[j].ToVector4() - row2[j].ToVector4();
						sum += (diff * diff).Sum();
					}
				}
			});

			return (float)Math.Sqrt(sum / channels);
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

		public static float Sum(this Vector4 val)
			=> val.X + val.Y + val.Z + val.W;

		#endregion
	}
}
