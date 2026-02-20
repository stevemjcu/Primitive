using System.Numerics;

namespace Primitive.Utility
{
    public static class RandomExtensions
    {
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
        {
            return rand.Next(2) * 2 - 1;
        }
    }
}
