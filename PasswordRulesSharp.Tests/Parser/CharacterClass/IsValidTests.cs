using NUnit.Framework;

using PasswordRulesSharp.Models;

namespace PasswordRulesSharp.Tests.Parser.CharacterClass
{
    public class IsValidTests
    {
        [TestCase("lower", 26)]
        [TestCase("upper", 26)]
        [TestCase("digit", 10)]
        [TestCase("ascii-printable", 95)]
        [TestCase("unicode", -1)]
        [TestCase("[-]", 1)]
        public void IsValid(string rawClass, int count)
        {
            Assert.True(Models.CharacterClass.TryParse(rawClass, out var parsedClass));

            switch (parsedClass)
            {
                case SpecificCharacterClass specific:
                    Assert.AreEqual(count, specific.Chars.Length);
                    break;
                case UnicodeCharacterClass unicode:
                    Assert.AreEqual("unicode", rawClass);
                    break;
            }
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
