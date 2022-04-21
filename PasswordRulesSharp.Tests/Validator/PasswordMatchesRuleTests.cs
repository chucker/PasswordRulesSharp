using NUnit.Framework;

namespace PasswordRulesSharp.Tests.Validator
{
    public class PasswordMatchesRuleTests
    {
        [TestCase("minlength: 20; required: lower; required: upper; required: digit; required: [-];", 
                  "asdfghQWERTY123456--")]
        [TestCase("minlength: 8",
                  "12345678")]
        [TestCase("minlength: 8; maxlength: 10",
                  "12345678")]
        [TestCase("minlength: 8; maxlength: 10; max-consecutive: 5",
                  "111112222")]
        public void MatchesRule(string rule, string password)
        {
            var parsedRule = new PasswordRulesSharp.Parser.Rule(rule);
            var validator = new PasswordRulesSharp.Validator.Validator(parsedRule);

            Assert.True(validator.PasswordIsValid(password, out _));
        }

        [TestCase("minlength: 20; required: lower; required: upper; required: digit; required: [-];",
                  "asdfghQWERTY123456-")]
        [TestCase("minlength: 8",
                  "1234567")]
        [TestCase("minlength: 8; maxlength: 10",
                  "1234567890-")]
        [TestCase("minlength: 8; maxlength: 10; max-consecutive: 4",
                  "111112222")]
        public void DoesNotMatchRule(string rule, string password)
        {
            var parsedRule = new PasswordRulesSharp.Parser.Rule(rule);
            var validator = new PasswordRulesSharp.Validator.Validator(parsedRule);

            Assert.False(validator.PasswordIsValid(password, out var failedRequirements));

            foreach (var item in failedRequirements)
            {
                TestContext.Out.WriteLine(item.ToString());
            }
        }
    }
}
