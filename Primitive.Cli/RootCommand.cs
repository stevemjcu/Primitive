using Primitive.Shapes;
using Primitive.Utility;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using Spectre.Console;
using Spectre.Console.Cli;
using System.Diagnostics;
using Color = SixLabors.ImageSharp.Color;

namespace Primitive.Cli
{
    internal class RootCommand : Command<Settings>
    {
        public override int Execute(CommandContext context, Settings settings, CancellationToken _)
        {
            using var input = Image.Load<Rgba32>(settings.InputPath);
            input.Mutate(x => x.Resize(new ResizeOptions { Size = new(settings.ComputationSize, settings.ComputationSize) }));

            var color = settings.Background == string.Empty ? Helper.AverageColor(input) : Color.Parse(settings.Background);
            using var model = new Model(input, color);

            void Step<T>() where T : IShape, new()
            {
                model.Add<T>(settings.Trials, settings.Failures);
            }

            var action = (Action)(settings.Shape switch
            {
                "Ellipse" => Step<Ellipse>,
                _ => throw new Exception(Resources.ErrorInvalidShape)
            });

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            AnsiConsole
                .Progress()
                .Columns(new TaskDescriptionColumn(), new ProgressBarColumn(), new PercentageColumn())
                .Start(ctx =>
                {
                    var task = ctx.AddTask(Resources.MessageProgress, true, settings.Iterations);
                    while (!ctx.IsFinished)
                    {
                        action.Invoke();
                        task.Increment(1);
                    }
                });

            stopwatch.Stop();
            AnsiConsole.MarkupLine(string.Format(Resources.MessageElapsedTime, stopwatch.Elapsed.ToString(@"hh\:mm\:ss")));

            using var output = new Image<Rgba32>(settings.OutputSize, settings.OutputSize, color);
            foreach (var s in model.Shapes) s.Draw(output);
            output.Save(settings.OutputPath);

            return 0;
        }
    }
}
