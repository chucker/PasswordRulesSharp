using NodaTime;

using PasswordRulesSharp.Models;
using PasswordRulesSharp.Rules;

using System.Linq;

namespace PasswordRulesSharp.Builder;

public class RuleBuilder
{
    private readonly IncompleteRule rule = new();

    public RuleBuilder MinLength(int? length)
    {
        rule.MinLength = length;

        return this;
    }

    public RuleBuilder MaxLength(int? length)
    {
        rule.MaxLength = length;

        return this;
    }

    public RuleBuilder MaxConsecutive(int? count)
    {
        rule.MaxConsecutive = count;

        return this;
    }

    public RuleBuilder ExpiresAfter(Period? period)
    {
        rule.ExpiresAfter = period;

        return this;
    }

    public RuleBuilder Require(CharacterClass characterClass)
    {
        rule.Required ??= new();

        // MAYBE: keep character classes in a set to avoid duplicate rules being added
        rule.Required.Add(characterClass);

        return this;
    }

    public RuleBuilder RequireUnicode() => Require(CharacterClass.Unicode);

    public RuleBuilder RequireAsciiPrintable() => Require(CharacterClass.AsciiPrintable);

    public RuleBuilder RequireDigit() => Require(CharacterClass.Digit);

    public RuleBuilder RequireLower() => Require(CharacterClass.Lower);

    public RuleBuilder RequireUpper() => Require(CharacterClass.Upper);

    public RuleBuilder RequireAnyOf(params char[] characters)
    {
        Require(new SpecificCharacterClass(characters));

        return this;
    }

    public RuleBuilder RequireAnyOf(string characters) => RequireAnyOf(characters.ToCharArray());

    public RuleBuilder RequireAnyOf(params CharacterClass[] classes)
    {
        Require(classes.Aggregate(CharacterClass.Combined));

        return this;
    }

    public IRule Build()
    {
        return new BuiltRule(
            rule.MinLength,
            rule.MaxLength,
            rule.MaxConsecutive,
            rule.ExpiresAfter,
            rule.Required
        );
    }
}
