using Primitive;
using Primitive.Shapes;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;
using Color = SixLabors.ImageSharp.Color;

var app = new CommandApp<RootCommand>();
app.Configure(c => c.SetExceptionHandler((ex, _) => AnsiConsole.WriteException(ex)));
return app.Run(args);

internal sealed class RootCommand : Command<RootCommand.Settings>
{
	public class Settings : CommandSettings
	{
		[Description("Path to input image")]
		[CommandArgument(0, "<InputPath>")]
		public required string InputPath { get; set; }

		[Description("Path to output image")]
		[CommandArgument(1, "<OutputPath>")]
		public required string OutputPath { get; set; }

		[Description("Type of shape")]
		[CommandArgument(2, "<Shape>")]
		public required string Shape { get; set; }

		[Description("Number of shapes")]
		[CommandArgument(3, "<Iterations>")]
		public required int Iterations { get; set; }

		[Description("Number of starting shapes per iteration")]
		[CommandOption("--trials")]
		[DefaultValue(200)]
		public int Trials { get; set; }

		[Description("Number of allowed consecutive failures per iteration")]
		[CommandOption("--limit")]
		[DefaultValue(30)]
		public int Limit { get; set; }

		[Description("Background color hex code; auto-detected if unspecified")]
		[CommandOption("--background")]
		public string? Background { get; set; }

		[Description("Dimension to resize input image to")]
		[CommandOption("--input-size")]
		[DefaultValue(256)]
		public int InputSize { get; set; }

		[Description("Dimension of output image")]
		[CommandOption("--output-size")]
		[DefaultValue(1024)]
		public int OutputSize { get; set; }
	}

	public override int Execute(CommandContext context, Settings settings)
	{
		using var input = Image.Load<Rgba32>(settings.InputPath);
		input.Mutate(x => x.Resize(new ResizeOptions
		{
			Size = new(settings.InputSize, settings.InputSize)
		}));

		var background = settings.Background is not null
			? Color.Parse(settings.Background)
			: Helper.AverageColor(input);

		var model = new Model(input, background);
		var shape = settings.Shape switch
		{
			"Ellipse" => (Shape)new Ellipse(),
			_ => throw new Exception("Invalid shape")
		};

		// Run model with progress bar
		AnsiConsole.Progress().Start(ctx =>
		{
			var task = ctx.AddTask("[green]Adding shapes[/]", true, settings.Iterations);
			while (!ctx.IsFinished)
			{
				model.Add(shape.New(), settings.Trials, settings.Limit);
				task.Increment(1);
			}
		});

		using var output = model.Export(settings.OutputSize, background);
		output.Save(settings.OutputPath);
		return 0;
	}
}