using Primitive;
using Primitive.Cli;
using Spectre.Console;
using Spectre.Console.Cli;

var app = new CommandApp<RootCommand>().WithDescription(Resources.DescriptionApp);
app.Configure(c => c.SetExceptionHandler((ex, _) => AnsiConsole.WriteException(ex)));
return app.Run(args);
