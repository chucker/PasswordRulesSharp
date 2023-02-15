using NUnit.Framework;

using PasswordRulesSharp.Rules;

using System.Linq;

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
        [TestCase("minlength: 8; maxlength: 10; max-consecutive: 2",
                  "12121212")]
        [TestCase("minlength: 8; maxlength: 10; max-consecutive: 5",
                  "111112222")]
        [TestCase("minlength: 8; required: lower,upper; required: digit",
            "aaaaa333")]
        [TestCase("minlength: 8; required: upper; required: digit",
            "BBBBB444")]
        [TestCase("minlength: 8; required: digit,upper",
            "12345678")]
        [TestCase("minlength: 8; required: digit,upper",
            "AAAAAAAA")]
        public void MatchesRule(string rule, string password)
        {
            var parsedRule = new StringRule(rule);
            var validator = new PasswordRulesSharp.Validator.Validator(parsedRule);

            Assert.True(validator.PasswordIsValid(password, out var requirements));

            foreach (var (Requirement, Success) in requirements.Where(rs => !rs.Success))
            {
                TestContext.Out.WriteLine($"Failed requirement: {Requirement}");
            }
        }

        [TestCase("required: lower",
                  "A")]
        [TestCase("required: upper",
                  "b")]
        [TestCase("required: [-]",
                  "!")]
        [TestCase("required: lower; required: upper; required: digit; required: [-];",
                  "12345678901234567890")]
        [TestCase("minlength: 8",
                  "1234567")]
        [TestCase("minlength: 8; maxlength: 10",
                  "1234567890-")]
        [TestCase("minlength: 8; maxlength: 10; max-consecutive: 1",
                  "11121212")]
        [TestCase("minlength: 8; maxlength: 10; max-consecutive: 4",
                  "111112222")]
        [TestCase("minlength: 8; required: lower,upper; required: digit",
            "11111111")]
        [TestCase("minlength: 8; required: digit,upper",
            "aaaaaaaa")]
        public void DoesNotMatchRule(string rule, string password)
        {
            var parsedRule = new StringRule(rule);
            var validator = new PasswordRulesSharp.Validator.Validator(parsedRule);

            Assert.False(validator.PasswordIsValid(password, out var requirements));

            foreach (var (Requirement, Success) in requirements.Where(rs => !rs.Success))
            {
                TestContext.Out.WriteLine($"Failed requirement: {Requirement}");
            }
        }
    }
}
