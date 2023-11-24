using PasswordRulesSharp.Cli.Commands;

using Spectre.Console.Cli;

var app = new CommandApp();

app.Configure(config =>
{
    config.AddCommand<GenerateCommand>("generate")
        .WithAlias("gen")
        .WithDescription("Generates a bunch of passwords based on a rule.");

#if DEBUG
    config.PropagateExceptions();
    config.ValidateExamples();
#endif
});

app.Run(args);
