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
        public static readonly SpecificCharacterClass AsciiPrintable = _AsciiPrintable.Value;

        private static readonly Lazy<SpecificCharacterClass> _Digit = new(() =>
        {
            var included = new List<char>();
            for (char c = '0'; c <= '9'; c++)
                included.Add(c);
            return new SpecificCharacterClass(included.ToArray());
        });
        public static readonly SpecificCharacterClass Digit = _Digit.Value;

        private static readonly Lazy<SpecificCharacterClass> _Lower = new(() =>
        {
            var included = new List<char>();
            for (char c = 'a'; c <= 'z'; c++)
                included.Add(c);
            return new SpecificCharacterClass(included.ToArray());
        });
        public static readonly SpecificCharacterClass Lower = _Lower.Value;

        public static readonly UnicodeCharacterClass Unicode = new();

        private static readonly Lazy<SpecificCharacterClass> _Upper = new(() =>
        {
            var included = new List<char>();
            for (char c = 'A'; c <= 'Z'; c++)
                included.Add(c);
            return new SpecificCharacterClass(included.ToArray());
        });
        public static readonly SpecificCharacterClass Upper = _Upper.Value;

        public static bool TryParse(string rawClasses, [NotNullWhen(true)] out CharacterClass[]? classes)
        {
            var included = new List<char>();
            var rawSplit = rawClasses.Split(',');

            if (rawSplit.Length == 0)
            {
                classes = null;
                return false;
            }

            var classesList = new List<CharacterClass>();
            foreach (var rawClass in rawSplit)
            {
                if (rawClass == "lower")
                {
                    classesList.Add(Lower);

                    continue;
                }

                if (rawClass == "upper")
                {
                    classesList.Add(Upper);

                    continue;
                }

                if (rawClass == "digit")
                {
                    classesList.Add(Digit);

                    continue;
                }

                if (rawClass == "ascii-printable")
                {
                    classesList.Add(AsciiPrintable);

                    continue;
                }

                if (rawClass == "unicode")
                {
                    classesList.Add(Unicode);

                    continue;
                }

                if (rawClass.StartsWith("[") && rawClass.EndsWith("]"))
                {
                    included.AddRange(rawClass[1..^1]);

                    classesList.Add(new SpecificCharacterClass(included.ToArray()));

                    continue;
                }
            }

            classes = classesList.ToArray();

            return classes.Length > 0;
        }
    }
}
