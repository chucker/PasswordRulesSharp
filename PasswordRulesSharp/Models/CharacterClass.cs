using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace PasswordRulesSharp.Models
{
    /// <summary>
    /// A character class is a set of characters (for example: all letters, all
    /// digits, or an explicit set of specific characters) from which a
    /// requirement can be created.
    /// </summary>
    public abstract class CharacterClass
    {
        private static readonly Lazy<SpecificCharacterClass> _AsciiPrintable = new(() =>
        {
            var included = new List<char>();
            for (char c = ' '; c <= '~'; c++)
                included.Add(c);
            return new SpecificCharacterClass(included.ToArray());
        });

        /// <summary>
        /// A built-in <see cref="CharacterClass"/> that contains any
        /// "printable ASCII" character, meaning all letters, digits, and
        /// special characters in ASCII, but not "non-printable" characters
        /// such as a line break.
        /// </summary>
        public static SpecificCharacterClass AsciiPrintable => _AsciiPrintable.Value;

        private static readonly Lazy<SpecificCharacterClass> _Digit = new(() =>
        {
            var included = new List<char>();
            for (char c = '0'; c <= '9'; c++)
                included.Add(c);
            return new SpecificCharacterClass(included.ToArray());
        });

        /// <summary>
        /// A <see cref="CharacterClass"/> that contains any decimal digit from
        /// <c>0</c> to (and including) <c>9</c>.
        /// </summary>
        public static SpecificCharacterClass Digit => _Digit.Value;

        private static readonly Lazy<SpecificCharacterClass> _Lower = new(() =>
        {
            var included = new List<char>();
            for (char c = 'a'; c <= 'z'; c++)
                included.Add(c);
            return new SpecificCharacterClass(included.ToArray());
        });

        /// <summary>
        /// A <see cref="CharacterClass"/> that contains lower-case Latin
        /// letters <c>a</c> through <c>z</c>.
        /// </summary>
        public static SpecificCharacterClass Lower => _Lower.Value;

        /// <summary>
        /// A <see cref="CharacterClass"/> that contains any Unicode character.
        /// (Strictly speaking, any Unicode grapheme cluster.)
        /// </summary>
        public static readonly UnicodeCharacterClass Unicode = new();

        private static readonly Lazy<SpecificCharacterClass> _Upper = new(() =>
        {
            var included = new List<char>();
            for (char c = 'A'; c <= 'Z'; c++)
                included.Add(c);
            return new SpecificCharacterClass(included.ToArray());
        });

        /// <summary>
        /// A <see cref="CharacterClass"/> that contains upper-case Latin
        /// letters <c>A</c> through <c>Z</c>.
        /// </summary>
        public static SpecificCharacterClass Upper => _Upper.Value;

        /// <summary>
        /// A <see cref="CharacterClass"/> that combines two existing character
        /// classes as a union, meaning any character in either class is valid.
        /// </summary>
        public static CharacterClass Combined(CharacterClass? left, CharacterClass? right)
        {
            if (left is null) throw new ArgumentNullException(nameof(left));
            if (right is null) throw new ArgumentNullException(nameof(right));

            if (left is UnicodeCharacterClass || right is UnicodeCharacterClass)
                return new UnicodeCharacterClass();

            if (left is SpecificCharacterClass leftSpecific && right is SpecificCharacterClass rightSpecific)
                return new SpecificCharacterClass(leftSpecific.Chars.Union(rightSpecific.Chars).ToArray());

            throw new ArgumentException("Cannot combine non-specific left and right character classes");
        }

        /// <summary>
        /// <para>
        /// Given a rule written in string form, tries to parse a token as one
        /// or more <see cref="CharacterClass"/>es, separated by a comma (,).
        /// </para>
        ///
        /// <para>
        /// Each class is expressed either with the keywords <c>lower</c>,
        /// <c>upper</c>, <c>digit</c>, <c>ascii-printable</c>, or
        /// <c>unicode</c>, with by a set of individual characters surrounded
        /// by square brackets ([]), e.g. <c>[abc]</c> for those three specific
        /// characters.</para>
        ///
        /// <para>
        /// See also:
        /// <seealso cref="https://developer.apple.com/documentation/security/password_autofill/customizing_password_autofill_rules"/>
        /// </para>
        /// </summary>
        public static bool TryParse(string rawClasses, [NotNullWhen(true)] out CharacterClass? classes)
        {
            var rawSplit = rawClasses.Split(',');
            if (rawSplit.Length == 0)
            {
                classes = null;

                return false;
            }

            if (rawSplit.Length == 1)
            {
                return TryParseSingle(rawSplit[0], out classes);
            }

            classes = rawSplit
                .Select(rawClass => (TryParseSingle(rawClass, out var parsedClass), parsedClass))
                .Where(t => t.Item1)
                .Select(t => t.parsedClass)
                .Aggregate(Combined);

            return classes != null;
        }

        private static bool TryParseSingle(string rawClass, [NotNullWhen(true)] out CharacterClass? @class)
        {
            if (rawClass == "lower")
            {
                @class = Lower;

                return true;
            }

            if (rawClass == "upper")
            {
                @class = Upper;

                return true;
            }

            if (rawClass == "digit")
            {
                @class = Digit;

                return true;
            }

            if (rawClass == "ascii-printable")
            {
                @class = AsciiPrintable;

                return true;
            }

            if (rawClass == "unicode")
            {
                @class = Unicode;

                return true;
            }

            const StringComparison invariantCulture = StringComparison.InvariantCulture;
            if (rawClass.StartsWith("[", invariantCulture) && rawClass.EndsWith("]", invariantCulture))
            {
                var included = rawClass[1..^1];

                @class = new SpecificCharacterClass(included.ToArray());

                return true;
            }

            @class = null;
            return false;
        }
    }
}
