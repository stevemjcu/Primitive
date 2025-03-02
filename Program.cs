using Primitive;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;
using Size = SixLabors.ImageSharp.Size;

return new CommandApp<RootCommand>().Run(args);

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
	}

	public override int Execute(CommandContext context, Settings settings)
	{
		AnsiConsole.MarkupLine($"Input: [blue]{settings.Input}[/]");
		AnsiConsole.MarkupLine($"Output: [blue]{settings.Output}[/]");
		AnsiConsole.MarkupLine($"Shape: [blue]{settings.Shape}[/]");
		AnsiConsole.MarkupLine($"Iterations: [blue]{settings.Iterations}[/]");

		var inputSize = new Size(settings.InputSize, settings.InputSize);
		var outputSize = new Size(settings.OutputSize, settings.OutputSize);

		using var input = Image.Load(settings.Input);
		// How to resize relative to center of image?
		input.Mutate(x => x.Resize(inputSize));

		var model = new Model<IShape>(input);
		for (var i = 0; i < settings.Iterations; i++) model.Add();
		var output = model.Process(outputSize);

		output.Save(settings.Output);
		return 0;
	}
}