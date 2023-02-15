using NUnit.Framework;

using PasswordRulesSharp.Rules;

using System.Linq;
using PasswordRulesSharp.Rules;

namespace PasswordRulesSharp.Tests.Generator
{
    public class ChooseCharsTests
    {
        [TestCase("minlength: 20; required: lower; required: upper; required: digit; required: [-];", new[] { 'a', 'Z', '5', '-' })]
        [TestCase("minlength: 8", new[] { 'a', 'Z', '5' })]
        [TestCase("minlength: 20; required: lower; required: upper; required: digit; required: [!];", new[] { 'a', 'Z', '5', '!' })]
        [TestCase("minlength: 20; maxlength: 30; required: lower; required: upper; required: digit; required: [-];", new[] { 'a', 'Z', '5', '-' })]
        [TestCase("required: upper", new[] { 'a', 'Z', '5' })]
        public void ChooseChars(string rule, char[] expectedChars)
        {
            var parsedRule = new StringRule(rule);

            var generator = new PasswordRulesSharp.Generator.Generator(parsedRule);
            var actualChars = generator.ChooseChars();

            foreach (var expectedChar in expectedChars)
            {
                Assert.That(actualChars.Any(c => c.Contains(expectedChar)), Is.True);
            }
        }

        [TestCase("minlength: 20; required: lower; max-consecutive: 1;", 1)]
        [TestCase("minlength: 20; required: lower; max-consecutive: 2;", 2)]
        public void ChooseNonConsecutiveChars(string rule, int maxConsecutive)
        {
            var parsedRule = new StringRule(rule);

            Assert.That(parsedRule.MaxConsecutive, Is.EqualTo(maxConsecutive));

            var generator = new PasswordRulesSharp.Generator.Generator(parsedRule);

            var password = generator.GeneratePassword();

            TestContext.WriteLine($"Generated: {password}");

            int consecutiveCount = 0;

            for (int i = 0; i < password.Length; i++)
            {
                if (i + 1 < password.Length && password[i] == password[i + 1])
                    consecutiveCount++;
                else
                    consecutiveCount = 0;

                Assert.That(consecutiveCount, Is.LessThanOrEqualTo(maxConsecutive));
            }

            //var actualChars = generator.ChooseChars();

            //foreach (var expectedChar in expectedChars)
            //{
            //    Assert.True(actualChars.Any(c => c.Contains(expectedChar)));
            //}
        }
    }
}