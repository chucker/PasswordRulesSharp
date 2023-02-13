using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

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

        public static bool TryParse(string rawClass, [NotNullWhen(true)] out CharacterClass? @class)
        {
            var included = new List<char>();

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

            if (rawClass.StartsWith("[") && rawClass.EndsWith("]"))
            {
                included.AddRange(rawClass[1..^1]);

                @class = new SpecificCharacterClass(included.ToArray());
                return true;
            }

            @class = null;
            return false;
        }
    }
}
