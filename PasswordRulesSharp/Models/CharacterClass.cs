using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace PasswordRulesSharp.Models
{
    public abstract class CharacterClass
    {
        private static readonly Lazy<SpecificCharacterClass> _AsciiPrintable = new(() =>
        {
            var included = new List<char>();
            for (char c = ' '; c <= '~'; c++)
                included.Add(c);
            return new SpecificCharacterClass(included.ToArray());
        });

        public static SpecificCharacterClass AsciiPrintable => _AsciiPrintable.Value;

        private static readonly Lazy<SpecificCharacterClass> _Digit = new(() =>
        {
            var included = new List<char>();
            for (char c = '0'; c <= '9'; c++)
                included.Add(c);
            return new SpecificCharacterClass(included.ToArray());
        });

        public static SpecificCharacterClass Digit => _Digit.Value;

        private static readonly Lazy<SpecificCharacterClass> _Lower = new(() =>
        {
            var included = new List<char>();
            for (char c = 'a'; c <= 'z'; c++)
                included.Add(c);
            return new SpecificCharacterClass(included.ToArray());
        });

        public static SpecificCharacterClass Lower => _Lower.Value;

        public static readonly UnicodeCharacterClass Unicode = new();

        private static readonly Lazy<SpecificCharacterClass> _Upper = new(() =>
        {
            var included = new List<char>();
            for (char c = 'A'; c <= 'Z'; c++)
                included.Add(c);
            return new SpecificCharacterClass(included.ToArray());
        });

        public static SpecificCharacterClass Upper => _Upper.Value;

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

        public static bool TryParseSingle(string rawClass, [NotNullWhen(true)] out CharacterClass? @class)
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