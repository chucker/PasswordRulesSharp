using NUnit.Framework;

using System.Linq;

namespace PasswordRulesSharp.Tests.Generator
{
    public class ChooseCharsTests
    {
        [TestCase("minlength: 20; required: lower; required: upper; required: digit; required: [-];", new[] { 'a', 'Z', '5', '-' })]
        [TestCase("minlength: 8", new[] { 'a', 'Z', '5' })]
        [TestCase("minlength: 20; required: lower; required: upper; required: digit; required: [!];", new[] { 'a', 'Z', '5', '!' })]
        [TestCase("minlength: 20; maxlength: 30; required: lower; required: upper; required: digit; required: [-];", new[] { 'a', 'Z', '5', '-' })]
        [TestCase("required: upper", new[] { 'a', 'Z', '5' })]
        [TestCase("", new[] { 'a', 'Z', '5' })] // we currently default to lower, upper, digit
        public void ChooseRequiredChars(string rule, char[] expectedChars)
        {
            var parsedRule = new PasswordRulesSharp.Parser.Rule(rule);

            var generator = new PasswordRulesSharp.Generator.Generator(parsedRule);
            var actualChars = generator.ChooseChars();

            foreach (var expectedChar in expectedChars)
            {
                Assert.True(actualChars.Any(c => c.Contains(expectedChar)), $"Expected '{expectedChar}'");
            }
        }

        [TestCase("required: upper; allowed: upper", new[] { 'Z' }, new[] { 'a', '5' })]
        [TestCase("allowed: lower; allowed: [!]", new[] { 'x' }, new[] { 'X', '5' })]
        public void ChooseAllowedChars(string rule, char[] expectedChars, char[] unexpectedChars)
        {
            var parsedRule = new PasswordRulesSharp.Parser.Rule(rule);

            var generator = new PasswordRulesSharp.Generator.Generator(parsedRule);
            var actualChars = generator.ChooseChars();

            foreach (var expectedChar in expectedChars)
            {
                Assert.True(actualChars.Any(c => c.Contains(expectedChar)), $"Expected '{expectedChar}'");
            }

            foreach (var unexpectedChar in unexpectedChars)
            {
                Assert.False(actualChars.Any(c => c.Contains(unexpectedChar)), $"Did not expect '{unexpectedChar}'");
            }
        }
    }
}
