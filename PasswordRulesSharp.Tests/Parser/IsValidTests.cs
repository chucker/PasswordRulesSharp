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
            var parser = new PasswordRulesSharp.Parser.Parser();

            var result = parser.IsValid(rule);

            Assert.True(result.Success);
            Assert.AreEqual(expectedCount, result.Count);
        }

        [TestCase("asd")]
        [TestCase("minlength8")]
        public void IsInvalid(string rule)
        {
            var parser = new PasswordRulesSharp.Parser.Parser();

            var result = parser.IsValid(rule);

            Assert.False(result.Success);
        }
    }
}
