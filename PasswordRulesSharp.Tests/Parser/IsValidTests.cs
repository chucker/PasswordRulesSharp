using NUnit.Framework;

using PasswordRulesSharp.Rules.Parsing;

namespace PasswordRulesSharp.Tests.Parser
{
    public class IsValidTests
    {
        [TestCase("minlength: 20; required: lower; required: upper; required: digit; required: [-];", 5)]
        [TestCase("minlength: 8", 1)]
        [TestCase("minlength: 8 ;", 1)]
        [TestCase(";minlength: 8", 1)]
        public void IsValid(string rule, int expectedCount)
        {
            var tokenizer = new Tokenizer(rule);

            var result = tokenizer.IsValid();

            Assert.Multiple(() =>
            {
                Assert.That(success, Is.True);
                Assert.That(count, Is.EqualTo(expectedCount));
            });
        }

        [TestCase("asd")]
        [TestCase("minlength8")]
        public void IsInvalid(string rule)
        {
            var tokenizer = new Tokenizer(rule);

            var result = tokenizer.IsValid();

            Assert.That(result.Success, Is.False);
        }
    }
}
