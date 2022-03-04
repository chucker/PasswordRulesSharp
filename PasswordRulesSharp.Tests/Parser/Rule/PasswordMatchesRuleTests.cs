using NUnit.Framework;

namespace PasswordRulesSharp.Tests.Parser.Rule
{
    public class PasswordMatchesRuleTests
    {
        [TestCase("minlength: 20; required: lower; required: upper; required: digit; required: [-];", 
                  "asdfghQWERTY123456--")]
        [TestCase("minlength: 8",
                  "12345678")]
        [TestCase("minlength: 8; maxlength: 10",
                  "12345678")]
        public void MatchesRule(string rule, string password)
        {
            var parsedRule = new PasswordRulesSharp.Parser.Rule(rule);

            Assert.True(parsedRule.PasswordMatchesRule(password, out _));
        }

        [TestCase("minlength: 20; required: lower; required: upper; required: digit; required: [-];",
                  "asdfghQWERTY123456-")]
        [TestCase("minlength: 8",
                  "1234567")]
        [TestCase("minlength: 8; maxlength: 10",
                  "1234567890-")]
        public void DoesNotMatchRule(string rule, string password)
        {
            var parsedRule = new PasswordRulesSharp.Parser.Rule(rule);

            Assert.False(parsedRule.PasswordMatchesRule(password, out var failedRequirements));

            foreach (var item in failedRequirements)
            {
                TestContext.Out.WriteLine(item.ToString());
            }
        }
    }
}
