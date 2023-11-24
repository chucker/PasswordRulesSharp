using System;
using System.ComponentModel;

using Spectre.Console;
using Spectre.Console.Cli;

using Rule = PasswordRulesSharp.Rules.Rule;

namespace PasswordRulesSharp.Cli.Commands;

public class GenerateCommand : Command<GenerateCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandOption("--rule")]
        public string Rule { get; set; } = "";

        [CommandOption("--count")]
        [DefaultValue(5)]
        public int Count { get; set; }
    }

    public override int Execute(CommandContext context, Settings settings)
    {
        var rule = Rule.FromString(settings.Rule);
        var generator = new Generator.Generator(rule);

        for (int i = 0; i < settings.Count; i++)
            Console.WriteLine(generator.GeneratePassword());

        return 0;
    }

    public override ValidationResult Validate(CommandContext context, Settings settings)
    {
        if (string.IsNullOrWhiteSpace(settings.Rule))
            return ValidationResult.Error("Please provide a valid rule, e.g. `--rule=\"minlength: 20; required: lower; required: upper; required: digit; required: [-];\"`.");

        return base.Validate(context, settings);
    }
}
