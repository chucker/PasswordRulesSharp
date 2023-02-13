using NUnit.Framework;

using PasswordRulesSharp.Models;

namespace PasswordRulesSharp.Tests.Parser.CharacterClass
{
    public class IsValidTests
    {
        [TestCase("lower", 26)]
        [TestCase("upper", 26)]
        [TestCase("upper,lower", 52)]
        [TestCase("digit", 10)]
        [TestCase("ascii-printable", 95)]
        [TestCase("unicode", -1)]
        [TestCase("[-]", 1)]
        public void IsValid(string rawClass, int count)
        {
            Assert.True(Models.CharacterClass.TryParse(rawClass, out var parsedClasses));

            var sum = 0;
            foreach (var parsedClass in parsedClasses)
            {
                switch (parsedClass)
                {
                    case SpecificCharacterClass specific:
                        sum += specific.Chars.Length;
                        break;
                    case UnicodeCharacterClass:
                        sum = -1;
                        Assert.AreEqual("unicode", rawClass);
                        break;
                }
            }

            Assert.AreEqual(count, sum);
        }

        [TestCase("asdf")]
        [TestCase("foo; bar")]
        [TestCase("baz: boop")]
        public void IsInvalid(string rawClass)
        {
            Assert.False(Models.CharacterClass.TryParse(rawClass, out _));
        }
    }
}
