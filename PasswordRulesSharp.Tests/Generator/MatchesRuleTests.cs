using NUnit.Framework;

namespace PasswordRulesSharp.Tests.Generator
{
    public class MatchesRuleTests
    {
        [TestCase("minlength: 20; required: lower; required: upper; required: digit; required: [-];")]
        [TestCase("minlength: 8")]
        [TestCase("minlength: 20; required: lower; required: upper; required: digit; required: [-];")]
        [TestCase("minlength: 20; maxlength: 30; required: lower; required: upper; required: digit; required: [-];")]
        [TestCase("minlength: 8; maxlength: 7")]
        [TestCase("minlength: 8 ; maxlength: 3")]
        [TestCase("required: upper")]
        public void MatchesRule(string rule)
        {
            var parsedRule = new PasswordRulesSharp.Parser.Rule(rule);

            var generator = new PasswordRulesSharp.Generator.Generator(parsedRule);

            var password = generator.GeneratePassword();

            Assert.AreEqual(generator.ChooseLength(), password.Length);

            TestContext.Out.WriteLine($"Generated '{password}'");
        }
    }
}
