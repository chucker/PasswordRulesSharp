using NUnit.Framework;

namespace PasswordRulesSharp.Tests.Rule
{
    public class PasswordIsValidTests
    {
        [TestCase("minlength: 20; required: lower; required: upper; required: digit; required: [-];", 
                  "asdfghQWERTY123456--")]
        [TestCase("minlength: 8",
                  "12345678")]
        [TestCase("minlength: 8; maxlength: 10",
                  "12345678")]
        public void IsValid(string rule, string password)
        {
            var parsedRule = new PasswordRulesSharp.Parser.Rule(rule);

            Assert.True(parsedRule.PasswordIsValid(password, out _));
        }

        [TestCase("minlength: 20; required: lower; required: upper; required: digit; required: [-];",
                  "asdfghQWERTY123456-")]
        [TestCase("minlength: 8",
                  "1234567")]
        [TestCase("minlength: 8; maxlength: 10",
                  "1234567890-")]
        public void IsInvalid(string rule, string password)
        {
            var parsedRule = new PasswordRulesSharp.Parser.Rule(rule);

            Assert.False(parsedRule.PasswordIsValid(password, out var failedRequirements));

            foreach (var item in failedRequirements)
            {
                TestContext.Out.WriteLine(item.ToString());
            }
        }
    }
}
