using NUnit.Framework;

using PasswordRulesSharp.Rules;

namespace PasswordRulesSharp.Tests.Parser.Rule
{
    public class StringRuleParserTests
    {
        [TestCase("minlength: 20; required: lower; required: upper; required: digit; required: [-];", 20)]
        [TestCase("minlength: 8", 8)]
        public void MinLength(string rule, int minLength)
        {
            var parsedRule = new StringRule(rule);

            Assert.That(parsedRule.MinLength, Is.EqualTo(minLength));
        }

        [TestCase("minlength: 20; required: lower; required: upper; required: digit; required: [-];", null)]
        [TestCase("minlength: 20; maxlength: 30; required: lower; required: upper; required: digit; required: [-];", 30)]
        [TestCase("minlength: 8; maxlength: 7", 7)]
        [TestCase("minlength: 8 ; maxlength: 3", 4)]
        public void MaxLength(string rule, int? maxLength)
        {
            var parsedRule = new StringRule(rule);

            Assert.That(parsedRule.MaxLength, Is.EqualTo(maxLength));
        }

        [TestCase("minlength: 20; required: lower; required: upper; required: digit; required: [-];", 20)]
        [TestCase("minlength: 10; maxlength: 30; required: lower; required: upper; required: digit; required: [-];", 10)]
        [TestCase("minlength: 8; maxlength: 7", 7)]
        public void FromString(string rule, int? minLength)
        {
            var parsedRule = PasswordRulesSharp.Rules.Rule.FromString(rule);

            Assert.That(parsedRule.MinLength, Is.EqualTo(minLength));
        }
    }
}
