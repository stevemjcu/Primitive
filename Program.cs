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
		[CommandArgument(1, "[Output]")]
		public required string Output { get; set; }
	}

	public override int Execute(CommandContext context, Settings settings)
	{
		AnsiConsole.MarkupLine($"Input: [blue]{settings.Input}[/]");
		AnsiConsole.MarkupLine($"Output: [blue]{settings.Output}[/]");
		return 0;
	}
}