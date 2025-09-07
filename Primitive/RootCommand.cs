using Primitive.Shapes;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;
using System.Diagnostics;
using Color = SixLabors.ImageSharp.Color;

namespace Primitive
{
	internal class RootCommand : Command<RootCommand.Settings>
	{
		public class ResourceDescriptionAttribute(string key)
			: DescriptionAttribute(Resources.ResourceManager.GetString(key)!)
		{ }

		public class Settings : CommandSettings
		{
			[ResourceDescription("DescriptionInputPath")]
			[CommandArgument(0, "<InputPath>")]
			public required string InputPath { get; set; }

			[ResourceDescription("DescriptionOutputPath")]
			[CommandArgument(1, "<OutputPath>")]
			public required string OutputPath { get; set; }

			[ResourceDescription("DescriptionShape")]
			[CommandArgument(2, "<Shape>")]
			public required string Shape { get; set; }

			[ResourceDescription("DescriptionIterations")]
			[CommandOption("--iterations")]
			[DefaultValue(200)]
			public int Iterations { get; set; }

			[ResourceDescription("DescriptionTrials")]
			[CommandOption("--trials")]
			[DefaultValue(200)]
			public int Trials { get; set; }

			[ResourceDescription("DescriptionFailures")]
			[CommandOption("--failures")]
			[DefaultValue(30)]
			public int Failures { get; set; }

			[ResourceDescription("DescriptionBackground")]
			[CommandOption("--background")]
			[DefaultValue("")]
			public required string Background { get; set; }

			[ResourceDescription("DescriptionComputationSize")]
			[CommandOption("--computation-size")]
			[DefaultValue(256)]
			public int ComputationSize { get; set; }

			[ResourceDescription("DescriptionOutputSize")]
			[CommandOption("--output-size")]
			[DefaultValue(512)]
			public int OutputSize { get; set; }
		}

		public override int Execute(CommandContext context, Settings settings)
		{
			using var input = Image.Load<Rgba32>(settings.InputPath);
			input.Mutate(x => x.Resize(new ResizeOptions { Size = new(settings.ComputationSize, settings.ComputationSize) }));

			var color = settings.Background == string.Empty ? Helper.AverageColor(input) : Color.Parse(settings.Background);
			using var model = new Model(input, color);

			void Step<T>() where T : IShape, new() => model.Add<T>(settings.Trials, settings.Failures);
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
