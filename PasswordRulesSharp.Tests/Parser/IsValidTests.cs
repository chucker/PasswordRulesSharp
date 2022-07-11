using NUnit.Framework;

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
            var tokenizer = new PasswordRulesSharp.Parser.Tokenizer(rule);

            var result = tokenizer.IsValid();

            Assert.True(result.Success);
            Assert.AreEqual(expectedCount, result.Count);
        }

        [TestCase("asd")]
        [TestCase("minlength8")]
        public void IsInvalid(string rule)
        {
            var tokenizer = new PasswordRulesSharp.Parser.Tokenizer(rule);

            var result = tokenizer.IsValid();

            Assert.False(result.Success);
        }
    }
}
