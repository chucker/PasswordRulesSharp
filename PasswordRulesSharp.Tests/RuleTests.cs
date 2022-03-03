using NUnit.Framework;

namespace PasswordRulesSharp.Tests
{
    public class RuleTests
    {
        [TestCase("minlength: 20; required: lower; required: upper; required: digit; required: [-];", 20)]
        [TestCase("minlength: 8", 8)]
        public void MinLength(string rule, int minLength)
        {
            var parsedRule = new PasswordRulesSharp.Rule(rule);

            Assert.AreEqual(minLength, parsedRule.MinLength);
        }

        [TestCase("minlength: 20; required: lower; required: upper; required: digit; required: [-];", null)]
        [TestCase("minlength: 20; maxlength: 30; required: lower; required: upper; required: digit; required: [-];", 30)]
        [TestCase("minlength: 8; maxlength: 7", 7)]
        [TestCase("minlength: 8 ; maxlength: 3", 4)]
        public void MaxLength(string rule, int? maxLength)
        {
            var parsedRule = new PasswordRulesSharp.Rule(rule);

            Assert.AreEqual(maxLength, parsedRule.MaxLength);
        }
    }
}
