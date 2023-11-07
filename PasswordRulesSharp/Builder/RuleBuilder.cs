using NodaTime;

using PasswordRulesSharp.Models;
using PasswordRulesSharp.Rules;

using System.Linq;

namespace PasswordRulesSharp.Builder;

/// <summary>
/// <see cref="RuleBuilder"/> lets you iteratively create a rule as a builder
/// pattern.
/// </summary>
public class RuleBuilder
{
    private readonly IncompleteRule rule = new();

    /// <summary>
    /// Appends a <see cref="Rule.MinLength"/> requirement to the rule you're
    /// building: at least how many characters long does the character need to
    /// be?
    /// </summary>
    public RuleBuilder MinLength(int? length)
    {
        rule.MinLength = length;

        return this;
    }

    /// <summary>
    /// Appends a <see cref="Rule.MaxLength"/> requirement to the rule you're
    /// building: at most how many characters long does the character habe to
    /// be?
    /// </summary>
    public RuleBuilder MaxLength(int? length)
    {
        rule.MaxLength = length;

        return this;
    }

    /// <summary>
    /// Appends a <see cref="Rule.MaxConsecutive"/> requirement to the rule
    /// you're building: how often can a character be repeated consecutively?
    /// </summary>
    public RuleBuilder MaxConsecutive(int? count)
    {
        rule.MaxConsecutive = count;

        return this;
    }

    /// <summary>
    /// Appends a <see cref="Rule.ExpiresAfter"/> requirement to the rule 
    /// you're building: how soon after creation will a password expire?
    /// </summary>
    public RuleBuilder ExpiresAfter(Period? period)
    {
        rule.ExpiresAfter = period;

        return this;
    }

    /// <summary>
    /// Appends a <see cref="Rule.Required"/> requirement to the rule
    /// you're building. At least one character from this class must appear.
    /// </summary>
    public RuleBuilder Require(CharacterClass characterClass)
    {
        rule.Required ??= new();

        // MAYBE: keep character classes in a set to avoid duplicate rules being added
        rule.Required.Add(characterClass);

        return this;
    }

    /// <summary>
    /// Appends a <see cref="Rule.Required"/> requirement for <c>Unicode</c> to
    /// the rule you're building. At least one character must be a Unicode
    /// character.
    /// </summary>
    public RuleBuilder RequireUnicode() => Require(CharacterClass.Unicode);

    /// <summary>
    /// Appends a <see cref="Rule.Required"/> requirement for <c>Unicode</c> to
    /// the rule you're building. At least one character must be a printable
    /// ASCII character.
    /// </summary>
    public RuleBuilder RequireAsciiPrintable() => Require(CharacterClass.AsciiPrintable);

    /// <summary>
    /// Appends a <see cref="Rule.Required"/> requirement for <c>Unicode</c> to
    /// the rule you're building. At least one character must be a digit.
    /// </summary>
    public RuleBuilder RequireDigit() => Require(CharacterClass.Digit);

    /// <summary>
    /// Appends a <see cref="Rule.Required"/> requirement for <c>Unicode</c> to
    /// the rule you're building. At least one character must be a lower-case
    /// Latin letter.
    /// </summary>
    public RuleBuilder RequireLower() => Require(CharacterClass.Lower);

    /// <summary>
    /// Appends a <see cref="Rule.Required"/> requirement for <c>Unicode</c> to
    /// the rule you're building. At least one character must be an upper-case
    /// Latin letter.
    /// </summary>
    public RuleBuilder RequireUpper() => Require(CharacterClass.Upper);

    /// <summary>
    /// Appends a <see cref="Rule.Required"/> requirement for <c>Unicode</c> to
    /// the rule you're building. At least one of the given characters must be
    /// used.
    /// </summary>
    public RuleBuilder RequireAnyOf(params char[] characters)
    {
        Require(new SpecificCharacterClass(characters));

        return this;
    }

    /// <summary>
    /// Appends a <see cref="Rule.Required"/> requirement for <c>Unicode</c> to
    /// the rule you're building. At least one of the given characters must be
    /// used.
    /// </summary>
    public RuleBuilder RequireAnyOf(string characters) => RequireAnyOf(characters.ToCharArray());

    /// <summary>
    /// Appends a <see cref="Rule.Required"/> requirement for <c>Unicode</c> to
    /// the rule you're building. At least one character in any of the given
    /// character classes must be used.
    /// </summary>
    public RuleBuilder RequireAnyOf(params CharacterClass[] classes)
    {
        Require(classes.Aggregate(CharacterClass.Combined));

        return this;
    }

    /// <summary>
    /// Builds the rule.
    /// </summary>
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
