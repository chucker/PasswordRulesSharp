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
            Assert.That(Models.CharacterClass.TryParse(rawClass, out var parsedClass), Is.True);

            switch (parsedClass)
            {
                case SpecificCharacterClass specific:
                    Assert.That(specific.Chars, Has.Length.EqualTo(count));
                    break;

                case UnicodeCharacterClass:
                    Assert.That(rawClass, Is.EqualTo("unicode"));
                    break;
            }
        }

        [TestCase("asdf")]
        [TestCase("foo; bar")]
        [TestCase("baz: boop")]
        public void IsInvalid(string rawClass)
        {
            Assert.That(Models.CharacterClass.TryParse(rawClass, out _), Is.False);
        }
    }
}
