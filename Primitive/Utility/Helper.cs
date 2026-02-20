using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.Numerics;

namespace Primitive.Utility
{
    public static class Helper
    {
        public static Color AverageColor(Image<Rgba32> image)
        {
            return AverageColor(image, image.Bounds);
        }

        public static Color AverageColor(Image<Rgba32> image, Rectangle area)
        {
            area = Rectangle.Intersect(image.Bounds, area);
            var sum = Vector4.Zero;
            image.ProcessPixelRows(rows =>
            {
                for (var i = area.Top; i < area.Bottom; i++)
                {
                    var row = rows.GetRowSpan(i);
                    for (var j = area.Left; j < area.Right; j++)
                    {
                        sum += row[j].ToVector4();
                    }
                }
            });
            return new Color(sum / (area.Width * area.Height));
        }

        public static float RootMeanSquareError(Image<Rgba32> source, Image<Rgba32> target)
        {
            var sum = SquareErrorSum(source, target, source.Bounds);
            return (float)Math.Sqrt(sum / (source.Height * source.Width * 4));
        }

        public static float RootMeanSquareError(
            Image<Rgba32> source, Image<Rgba32> target, Image<Rgba32> prevSource, float prevError, Rectangle area)
        {
            area = Rectangle.Intersect(prevSource.Bounds, area);
            var channels = prevSource.Height * prevSource.Width * 4;

            var sum = (float)Math.Pow(prevError, 2) * channels;
            sum -= SquareErrorSum(prevSource, target, area);
            sum += SquareErrorSum(source, target, area);
            return (float)Math.Sqrt(sum / channels);
        }

        private static float SquareErrorSum(Image<Rgba32> source, Image<Rgba32> target, Rectangle area)
        {
            var sum = 0f;
            source.ProcessPixelRows(target, (rows1, rows2) =>
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
            return sum;
        }

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
        {
            return val.X + val.Y + val.Z + val.W;
        }
    }
}
