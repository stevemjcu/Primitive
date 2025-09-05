using Primitive;
using Spectre.Console;
using Spectre.Console.Cli;

internal class Program
{
	private static int Main(string[] args)
	{
		var app = new CommandApp<RootCommand>().WithDescription(Resources.DescriptionApp);
		app.Configure(c => c.SetExceptionHandler((ex, _) => AnsiConsole.WriteException(ex)));
		return app.Run(args);
	}
}