using Primitive;
using Primitive.Shapes;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;
using System.Diagnostics;
using Color = SixLabors.ImageSharp.Color;

var app = new CommandApp<RootCommand>().WithDescription(Resources.DescriptionApp);
app.Configure(c => c.SetExceptionHandler((ex, _) => AnsiConsole.WriteException(ex)));
return app.Run(args);

internal sealed class RootCommand : Command<RootCommand.Settings>
{
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
		[CommandArgument(3, "<Iterations>")]
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
		[DefaultValue(1024)]
		public int OutputSize { get; set; }
	}

	public sealed class ResourceDescriptionAttribute(string key)
		: DescriptionAttribute(Resources.ResourceManager.GetString(key)!)
	{ }

	public override int Execute(CommandContext context, Settings settings)
	{
		using var input = Image.Load<Rgba32>(settings.InputPath);
		input.Mutate(x => x.Resize(new ResizeOptions
		{
			Size = new(settings.ComputationSize, settings.ComputationSize)
		}));

		var model = settings.Background == string.Empty
			? new Model(input)
			: new Model(input, Color.Parse(settings.Background));

		var action = (Action)(settings.Shape switch
		{
			"Ellipse" => () => model.Add<Ellipse>(settings.Trials, settings.Failures),
			_ => throw new Exception(Resources.ErrorInvalidShape)
		});

		var stopwatch = new Stopwatch();
		stopwatch.Start();

		AnsiConsole.Progress().Start(ctx =>
		{
			var task = ctx.AddTask(Resources.DescriptionProgress, true, settings.Iterations);
			while (!ctx.IsFinished)
			{
				action.Invoke();
				task.Increment(1);
			}
		});

		stopwatch.Stop();
		AnsiConsole.WriteLine($"Elapsed time: {stopwatch.Elapsed:c}");

		using var output = model.Export(settings.OutputSize);
		output.Save(settings.OutputPath);
		return 0;
	}
}