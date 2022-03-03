using NUnit.Framework;

namespace PasswordRulesSharp.Tests.Parser
{
    public class IsValidTests
    {
        [TestCase("minlength: 20; required: lower; required: upper; required: digit; required: [-];")]
        public void IsValid(string rule)
        {
            var parser = new PasswordRulesSharp.Parser();
            Assert.True(parser.IsValid(rule));
        }

        [TestCase("asd")]
        public void IsInvalid(string rule)
        {
            var parser = new PasswordRulesSharp.Parser();
            Assert.False(parser.IsValid(rule));
        }
    }
}
