using NUnit.Framework;

namespace PasswordRulesSharp.Tests.Generator
{
    public class ChooseLengthTests
    {
        [TestCase("minlength: 20; required: lower; required: upper; required: digit; required: [-];", 20)]
        [TestCase("minlength: 8", 20)]
        [TestCase("minlength: 20; required: lower; required: upper; required: digit; required: [-];", 20)]
        [TestCase("minlength: 20; maxlength: 30; required: lower; required: upper; required: digit; required: [-];", 20)]
        [TestCase("minlength: 8; maxlength: 7", 7)]
        [TestCase("minlength: 8 ; maxlength: 3", 4)]
        [TestCase("required: upper", 20)]
        public void ChooseLength(string rule, int expectedLength)
        {
            var parsedRule = new PasswordRulesSharp.Parser.Rule(rule);

            var generator = new PasswordRulesSharp.Generator.Generator(parsedRule);

            Assert.AreEqual(expectedLength, generator.ChooseLength());
        }
    }
}
