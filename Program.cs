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
app.WithDescription("A tool which recreates images using geometric shapes");
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
		public int Iterations { get; set; }

		[Description("Number of shapes to trial before starting iteration")]
		[CommandOption("--trials")]
		[DefaultValue(200)]
		public int Trials { get; set; }

		[Description("Number of consecutive failures before ending iteration")]
		[CommandOption("--failures")]
		[DefaultValue(30)]
		public int Failures { get; set; }

		[Description("Background color hex code")]
		[CommandOption("--background")]
		[DefaultValue("")]
		public required string Background { get; set; }

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

		var model = settings.Background == string.Empty
			? new Model(input)
			: new Model(input, Color.Parse(settings.Background));

		var action = (Action)(settings.Shape switch
		{
			"Ellipse" => () => model.Add<Ellipse>(settings.Trials, settings.Failures),
			_ => throw new Exception("Invalid shape")
		});

		AnsiConsole.Progress().Start(ctx =>
		{
			var task = ctx.AddTask("[green]Adding shapes[/]", true, settings.Iterations);
			while (!ctx.IsFinished)
			{
				action.Invoke();
				task.Increment(1);
			}
		});

		using var output = model.Export(settings.OutputSize);
		output.Save(settings.OutputPath);
		return 0;
	}
}