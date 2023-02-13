using NUnit.Framework;

namespace PasswordRulesSharp.Tests.Generator
{
    public class GeneratedPasswordTests
    {
        [TestCase("minlength: 20; required: lower; required: upper; required: digit; required: [-];")]
        [TestCase("minlength: 8")]
        [TestCase("minlength: 20; required: lower; required: upper; required: digit; required: [-];")]
        [TestCase("minlength: 20; maxlength: 30; required: lower; required: upper; required: digit; required: [-];")]
        [TestCase("minlength: 8; maxlength: 7")]
        [TestCase("minlength: 8 ; maxlength: 3")]
        [TestCase("required: upper")]
        public void LengthMatchesExpected(string rule)
        {
            var parsedRule = new PasswordRulesSharp.Parser.Rule(rule);

            var generator = new PasswordRulesSharp.Generator.Generator(parsedRule);

            var password = generator.GeneratePassword();

            Assert.AreEqual(generator.ChooseLength(), password.Length);

            TestContext.Out.WriteLine($"Generated '{password}'");
        }

        [TestCase("required: lower")]
        public void RequiredLowerContainsIt(string rule)
        {
            var parsedRule = new PasswordRulesSharp.Parser.Rule(rule);
            var generator = new PasswordRulesSharp.Generator.Generator(parsedRule);
            var password = generator.GeneratePassword();

            TestContext.Out.WriteLine($"Generated password: {password}");

            bool found = false;
            foreach (var item in new[] { "a", "b", "c", "d", "e",
                                         "f", "g", "h", "i", "j",
                                         "k", "l", "m", "n", "o",
                                         "p", "q", "r", "s", "t",
                                         "u", "v", "w", "x", "y", "z"})
            {
                if (password.Contains(item))
                {
                    TestContext.Out.WriteLine($"Password contains {item}");
                    found = true;
                    break;
                }
            }

            Assert.True(found);
        }

        [TestCase("required: upper")]
        public void RequiredUpperContainsIt(string rule)
        {
            var parsedRule = new PasswordRulesSharp.Parser.Rule(rule);
            var generator = new PasswordRulesSharp.Generator.Generator(parsedRule);
            var password = generator.GeneratePassword();

            TestContext.Out.WriteLine($"Generated password: {password}");

            bool found = false;
            foreach (var item in new[] { "A", "B", "C", "D", "E",
                                         "F", "G", "H", "I", "J",
                                         "K", "L", "M", "N", "O",
                                         "P", "Q", "R", "S", "T",
                                         "U", "V", "W", "X", "Y", "Z"})
            {
                if (password.Contains(item))
                {
                    TestContext.Out.WriteLine($"Password contains {item}");
                    found = true;
                    break;
                }
            }

            Assert.True(found);
        }

        [TestCase("required: digit")]
        public void RequiredDigitContainsIt(string rule)
        {
            var parsedRule = new PasswordRulesSharp.Parser.Rule(rule);
            var generator = new PasswordRulesSharp.Generator.Generator(parsedRule);
            var password = generator.GeneratePassword();

            TestContext.Out.WriteLine($"Generated password: {password}");

            bool found = false;
            foreach (var item in new[] { "0", "1", "2", "3", "4",
                                         "5", "6", "7", "8", "9" })
            {
                if (password.Contains(item))
                {
                    TestContext.Out.WriteLine($"Password contains {item}");
                    found = true;
                    break;
                }
            }

            Assert.True(found);
        }

        [TestCase("!")]
        [TestCase("$")]
        [TestCase("!@#$%")]
        public void RequiredSpecialCharsContainsOneOfThem(string chars)
        {
            var rule = $"required: [{chars}]";

            var parsedRule = new PasswordRulesSharp.Parser.Rule(rule);
            var generator = new PasswordRulesSharp.Generator.Generator(parsedRule);
            var password = generator.GeneratePassword();

            TestContext.Out.WriteLine($"Rule: {rule}");
            TestContext.Out.WriteLine($"Generated password: {password}");

            bool found = false;
            foreach (var item in chars)
            {
                if (password.Contains(item.ToString()))
                {
                    TestContext.Out.WriteLine($"Password contains {item}");
                    found = true;
                    break;
                }
            }

            Assert.True(found);
        }
    }
}
