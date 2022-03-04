using NUnit.Framework;

namespace PasswordRulesSharp.Tests.Generator
{
    public class ChooseCharsTests
    {
        //[TestCase("minlength: 20; required: lower; required: upper; required: digit; required: [-];", new[] { 'a', 'Z', '5', '-' })]
        [TestCase("minlength: 8", new[] { 'a', 'Z', '5' })]
        //[TestCase("minlength: 20; required: lower; required: upper; required: digit; required: [!];", new[] { 'a', 'Z', '5', '!' })]
        //[TestCase("minlength: 20; maxlength: 30; required: lower; required: upper; required: digit; required: [-];", new[] { 'a', 'Z', '5', '-' })]
        [TestCase("required: upper", new[] { 'a', 'Z', '5' })]
        public void ChooseChars(string rule, char[] expectedChars)
        {
            var parsedRule = new PasswordRulesSharp.Parser.Rule(rule);

            var generator = new PasswordRulesSharp.Generator.Generator(parsedRule);
            char[] actualChars = generator.ChooseChars();

            foreach (var item in expectedChars)
            {
                CollectionAssert.Contains(actualChars, item);
            }
        }
    }
}
