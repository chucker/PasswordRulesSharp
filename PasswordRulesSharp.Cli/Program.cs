using PasswordRulesSharp.Cli.Commands;

using Spectre.Console.Cli;

var app = new CommandApp();

app.Configure(config =>
{
    config.AddCommand<GenerateCommand>("generate")
        .WithAlias("gen")
        .WithDescription("Generates a bunch of passwords based on a rule.")
        .WithExample("generate", "--rule=\"minlength: 32\"")
        .WithExample("generate", "--rule=\"minlength: 20; required: lower; required: upper; required: digit; required: [-];\"");

#if DEBUG
    config.PropagateExceptions();
    config.ValidateExamples();
#endif
});

app.Run(args);
