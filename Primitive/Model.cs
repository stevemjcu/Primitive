using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Primitive
{
	internal class Model : IDisposable
	{
		private record State(IShape Shape, Image<Rgba32> Image, float Score = float.MaxValue);

		/// <summary>
		/// The image the model is trying to recreate.
		/// </summary>
		public Image<Rgba32> Target { get; }

		/// <summary>
		/// The image the model is developing.
		/// </summary>
		public Image<Rgba32> Current { get; private set; }

		/// <summary>
		/// The Root Mean Square Error (RMSE) between the current and target images (lower is better).
		/// </summary>
		public float Score { get; private set; }

		/// <summary>
		/// The ordered list of shapes which comprise the current image.
		/// </summary>
		public Queue<IShape> Shapes { get; } = [];

		public Model(Image<Rgba32> target, Color background)
		{
			Target = target;
			Current = new(target.Width, target.Height, background);
			Score = Helper.RootMeanSquareError(Current, target);
		}

		public void Dispose()
		{
			Target.Dispose();
			Current.Dispose();
		}

		/// <summary>
		/// Adds a shape to the model using a hillclimbing algorithm.<br />
		/// 1. Randomly generates shapes and determines the best one.<br />
		/// 2. Randomly mutates the shape until reaching a local maxima.
		/// </summary>
		/// <typeparam name="T">The type of shape to use.</typeparam>
		/// <param name="trials">The number of shapes to generate initially.</param>
		/// <param name="failures">The number of suboptimal mutations before stopping.</param>
		public void Add<T>(int trials, int failures) where T : IShape, new()
		{
			var best = OptimizeShape(TrialShapes<T>(trials), failures);
			Current = best.Image;
			Score = best.Score;
			Shapes.Enqueue(best.Shape);
		}

		private State TrialShapes<T>(int count) where T : IShape, new()
		{
			var bestLock = new object();
			var best = new State(new T(), Current.Clone());

			Parallel.For(0, count, i =>
			{
				var shape = new T();
				var next = Current.Clone();

				shape.Sample(Target);
				shape.Draw(next);

				var score = Helper.RootMeanSquareError(
					Current, next, Target, shape.Bounds(next.Bounds), Score);

				lock (bestLock)
				{
					if (score < best.Score)
						best = new(shape, next, score);
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
				var next = Current.Clone();

				shape.Mutate();
				shape.Draw(next);

				var score = Helper.RootMeanSquareError(
					Current, next, Target, shape.Bounds(next.Bounds), Score);

				if (score < best.Score)
					(best, i) = (new(shape, next, score), 0);
			}

			return best;
		}
	}
}
