using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Primitive
{
    public class Model : IDisposable
    {
        private record State(IShape Shape, Image<Rgba32> Image, float Error = float.MaxValue);

        /// <summary>
        /// The image the model is trying to recreate.
        /// </summary>
        public Image<Rgba32> Target { get; }

        /// <summary>
        /// The image the model is developing.
        /// </summary>
        public Image<Rgba32> Current { get; private set; }

        /// <summary>
        /// The difference between the current and target images.
        /// </summary>
        public float Error { get; private set; }

        /// <summary>
        /// The ordered list of shapes which comprise the current image.
        /// </summary>
        public List<IShape> Shapes { get; } = [];

        /// <summary>
        /// Initializes a new instance of the <see cref="Model"/> class.
        /// </summary>
        /// <param name="target">The input image.</param>
        /// <param name="background">The initial color of the output image.</param>
        public Model(Image<Rgba32> target, Color background)
        {
            Target = target;
            Current = new(target.Width, target.Height, background);
            Error = Helper.RootMeanSquareError(Current, target);
        }

        public void Dispose()
        {
            Target.Dispose();
            Current.Dispose();
        }

        /// <summary>
        /// Adds an <see cref="IShape"/> to the model using a hill climbing algorithm.<br />
        /// 1. Randomly generates shapes and determines the best one.<br />
        /// 2. Randomly mutates the shape until reaching a local maxima.
        /// </summary>
        /// <typeparam name="T">The type of shape to use.</typeparam>
        /// <param name="trials">The number of shapes to generate before starting.</param>
        /// <param name="failures">The number of suboptimal mutations before stopping.</param>
        public void Add<T>(int trials, int failures) where T : IShape, new()
        {
            var best = OptimizeShape(TrialShapes<T>(trials), failures);
            Current = best.Image;
            Error = best.Error;
            Shapes.Add(best.Shape);
        }

        private State TrialShapes<T>(int count) where T : IShape, new()
        {
            var bestLock = new object();
            var best = new State(new T(), Current.Clone());

            Parallel.For(0, count, i =>
            {
                var shape = new T();
                var image = Current.Clone();

                shape.Sample(Target);
                shape.Draw(image);

                var error = Helper.RootMeanSquareError(
                    image, Target, Current, Error, shape.Bounds(image.Bounds));

                lock (bestLock)
                {
                    if (error < best.Error)
                        best = new(shape, image, error);
                }
            });

            return best;
        }

        private State OptimizeShape(State start, int limit)
        {
            var best = start;

            for (var i = 0; i < limit; i++)
            {
                var shape = best.Shape.Clone();
                var image = Current.Clone();

                shape.Mutate();
                shape.Draw(image);

                var error = Helper.RootMeanSquareError(
                    image, Target, Current, Error, shape.Bounds(image.Bounds));

                if (error < best.Error)
                    (best, i) = (new(shape, image, error), 0);
            }

            return best;
        }
    }
}
