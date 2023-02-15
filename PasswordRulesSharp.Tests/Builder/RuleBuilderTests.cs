using NodaTime;

using NUnit.Framework;

using PasswordRulesSharp.Models;

using System.Linq;

namespace PasswordRulesSharp.Tests.Builder;

public class RuleBuilderTests
{
    [Test]
    public void BuildEmpty()
    {
        var rule = new PasswordRulesSharp.Builder.RuleBuilder().Build();

        Assert.That(rule.Required, Is.Null);
        Assert.That(rule.ExpiresAfter, Is.Null);
        Assert.That(rule.MaxConsecutive, Is.Null);
        Assert.That(rule.MaxLength, Is.Null);
        Assert.That(rule.MinLength, Is.Null);
    }

    [Test]
    public void BuildComplex()
    {
        var rule = new PasswordRulesSharp.Builder.RuleBuilder()
            .MinLength(8)
            .MaxLength(128)
            .MaxConsecutive(5)
            .RequireDigit()
            .RequireAnyOf(CharacterClass.Lower, CharacterClass.Upper)
            .RequireAnyOf(@".-*&/\@")
            .RequireAsciiPrintable()
            .ExpiresAfter(Period.FromMonths(3))
            .Build();

        Assert.That(rule.Required, Is.Not.Null);
        Assert.That(rule.Required!, Has.Count.EqualTo(4));
        Assert.That(rule.Required!.Any(c => c is SpecificCharacterClass specific && specific.Chars.SequenceEqual(new[] { '.', '-', '*', '&', '/', '\\', '@' })));
        Assert.That(rule.MinLength, Is.EqualTo(8));
        Assert.That(rule.MaxLength, Is.EqualTo(128));
        Assert.That(rule.MaxConsecutive, Is.EqualTo(5));
        Assert.That(rule.ExpiresAfter, Is.EqualTo(Period.FromMonths(3)));
    }
}
