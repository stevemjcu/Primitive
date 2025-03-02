using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;

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

		[Description("Background color by hex code; averaged if not specified")]
		[CommandOption("--background")]
		public string? Background { get; set; }

		[Description("Alpha value of shapes; optimized for if not specified")]
		[CommandOption("--alpha")]
		public int? Alpha { get; set; }

		[Description("Dimension to resize input image")]
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

		// TODO: Configure & run model then process output

		return 0;
	}
}