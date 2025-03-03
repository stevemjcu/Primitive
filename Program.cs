using Primitive;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;
using System.Diagnostics;
using Color = SixLabors.ImageSharp.Color;
using Size = SixLabors.ImageSharp.Size;

var app = new CommandApp<RootCommand>();
app.Configure(c => c.SetExceptionHandler((ex, _) => AnsiConsole.WriteException(ex)));
return app.Run(args);

internal sealed class RootCommand : Command<RootCommand.Settings>
{
	public class Settings : CommandSettings
	{
		[Description("Path to input image")]
		[CommandArgument(0, "<Input>")]
		public required string Input { get; set; }

		[Description("Path to output image")]
		[CommandArgument(1, "<Output>")]
		public required string Output { get; set; }

		[Description("Type of shape")]
		[CommandArgument(2, "<Shape>")]
		public required string Shape { get; set; }

		[Description("Number of shapes")]
		[CommandArgument(3, "<Iterations>")]
		public required int Iterations { get; set; }

		[Description("Background color hex code; averaged if unspecified")]
		[CommandOption("--background")]
		public string? Background { get; set; }

		[Description("Alpha value of each shape; optimized for if unspecified")]
		[CommandOption("--alpha")]
		public int? Alpha { get; set; }

		[Description("Dimension to resize input image to")]
		[CommandOption("--input-size")]
		[DefaultValue(256)]
		public int InputSize { get; set; }

		[Description("Dimension of output image")]
		[CommandOption("--output-size")]
		[DefaultValue(1024)]
		public int OutputSize { get; set; }

		[Description("Attaches a debugger to the process")]
		[CommandOption("--debug")]
		[DefaultValue(false)]
		public bool Debug { get; set; }
	}

	public override int Execute(CommandContext context, Settings settings)
	{
		if (settings.Debug) Debugger.Launch();

		var inputSize = new Size(settings.InputSize, settings.InputSize);
		var outputSize = new Size(settings.OutputSize, settings.OutputSize);

		using var input = Image.Load<Rgba32>(settings.Input);
		input.Mutate(x => x.Resize(new ResizeOptions { Size = inputSize }));

		var background = settings.Background is not null
			? Color.Parse(settings.Background)
			: Helper.AverageColor(input);

		AnsiConsole.MarkupLine($"Background color: [blue]{background.ToHex()}[/]");

		var model = new Model<IShape>(input, background);
		for (var i = 0; i < settings.Iterations; i++) model.Add();
		using var output = model.Process(outputSize);

		output.Save(settings.Output);
		return 0;
	}
}