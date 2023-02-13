using System.Linq;
using NodaTime;
using PasswordRulesSharp.Models;
using PasswordRulesSharp.Parser;

namespace PasswordRulesSharp.Builder;

public class Builder
{
    private readonly IncompleteRule rule = new();

    public Builder MinLength(int? length)
    {
        rule.MinLength = length;

        return this;
    }

    public Builder MaxLength(int? length)
    {
        rule.MaxLength = length;

        return this;
    }

    public Builder MaxConsecutive(int? count)
    {
        rule.MaxConsecutive = count;

        return this;
    }

    public Builder ExpiresAfter(Period? period)
    {
        rule.ExpiresAfter = period;

        return this;
    }

    public Builder Require(CharacterClass characterClass)
    {
        rule.Required ??= new();

        // MAYBE: keep character classes in a set to avoid duplicate rules being added
        rule.Required.Add(characterClass);

        return this;
    }

    public Builder RequireUnicode() => Require(CharacterClass.Unicode);

    public Builder RequireAsciiPrintable() => Require(CharacterClass.AsciiPrintable);

    public Builder RequireDigit() => Require(CharacterClass.Digit);

    public Builder RequireLower() => Require(CharacterClass.Lower);

    public Builder RequireUpper() => Require(CharacterClass.Upper);

    public Builder RequireAnyOf(params char[] characters)
    {
        Require(new SpecificCharacterClass(characters));

        return this;
    }

    public Builder RequireAnyOf(string characters) => RequireAnyOf(characters.ToCharArray());

    public Builder RequireAnyOf(params CharacterClass[] classes)
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
