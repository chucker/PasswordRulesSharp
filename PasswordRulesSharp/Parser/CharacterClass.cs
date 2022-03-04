using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace PasswordRulesSharp.Parser
{
    public class CharacterClass
    {
        private static readonly Lazy<CharacterClass> _Lower = new(() =>
        {
            var included = new List<char>();
            for (char c = 'a'; c <= 'z'; c++)
                included.Add(c);
            return new CharacterClass(included.ToArray());
        });
        public static readonly CharacterClass Lower = _Lower.Value;

        private static readonly Lazy<CharacterClass> _Upper = new(() =>
        {
            var included = new List<char>();
            for (char c = 'A'; c <= 'Z'; c++)
                included.Add(c);
            return new CharacterClass(included.ToArray());
        });
        public static readonly CharacterClass Upper = _Upper.Value;

        private static readonly Lazy<CharacterClass> _Digit = new(() =>
        {
            var included = new List<char>();
            for (char c = '0'; c <= '9'; c++)
                included.Add(c);
            return new CharacterClass(included.ToArray());
        });
        public static readonly CharacterClass Digit = _Digit.Value;

        public char[] Included { get; }

        private CharacterClass(char[] included)
        {
            Included = included;
        }

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

            if (rawClass.StartsWith("[") && rawClass.EndsWith("]"))
            {
                included.AddRange(rawClass[1..^1]);

                @class = new CharacterClass(included.ToArray());
                return true;
            }

            @class = null;
            return false;
        }
    }
}