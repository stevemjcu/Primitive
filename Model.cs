﻿using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Primitive
{
	internal class Model
	{
		private record State(IShape Shape, float Error = float.MaxValue);

		private readonly Image<Rgba32> Target;
		private readonly Image<Rgba32> Canvas;
		private float Error;

		private readonly Queue<IShape> Shapes = [];
		private readonly Color Background;

		public Model(Image<Rgba32> target)
			: this(target, Helper.AverageColor(target))
		{ }

		public Model(Image<Rgba32> target, Color background)
		{
			Target = target;
			Canvas = new(target.Width, target.Height, background);
			Error = Helper.RootMeanSquareError(Canvas, target);
			Background = background;
		}

		public void Add<T>(int trials, int failures) where T : IShape, new()
		{
			var state = Optimize(Trial<T>(trials), failures);
			state.Shape.Draw(Canvas);
			Error = state.Error;
			Shapes.Enqueue(state.Shape);
		}

		private State Trial<T>(int n) where T : IShape, new()
		{
			var @lock = new object();
			var best = new State(new T());
			Parallel.For(0, n, i =>
			{
				var shape = new T();
				var canvas = Canvas.Clone();

				shape.Sample(Target);
				shape.Draw(canvas);

				var error = Helper.RootMeanSquareError(
					Canvas, canvas, Target,
					shape.Bounds(canvas.Bounds), Error);

				lock (@lock)
					if (error < best.Error)
						best = new(shape, error);
			});
			return best;
		}

		private State Optimize(State start, int n)
		{
			var best = start;
			for (var i = 0; i < n; i++)
			{
				var shape = best.Shape.Clone();
				var canvas = Canvas.Clone();

				shape.Mutate();
				shape.Draw(canvas);

				var error = Helper.RootMeanSquareError(
					Canvas, canvas, Target,
					shape.Bounds(canvas.Bounds), Error);

				if (error < best.Error)
					(best, i) = (new(shape, error), 0);
			}
			return best;
		}

		public Image<Rgba32> Export(int size)
		{
			var image = new Image<Rgba32>(size, size, Background);
			foreach (var s in Shapes) s.Draw(image);
			return image;
		}
	}
}
