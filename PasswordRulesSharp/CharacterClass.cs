using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace PasswordRulesSharp
{
    public class CharacterClass
    {
        public char[] Included { get; }

        private CharacterClass(char[] included) {
            Included = included;
        }

        public static bool TryParse(string rawClass, [NotNullWhen(true)] out CharacterClass? @class)
        {
            var included = new List<char>();

            if (rawClass == "lower")
            {
                for (char c = 'a'; c <= 'z'; c++)
                    included.Add(c);

                @class = new CharacterClass(included.ToArray());
                    return true;
            }

            if (rawClass == "upper")
            {
                for (char c = 'A'; c <= 'Z'; c++)
                    included.Add(c);

                @class = new CharacterClass(included.ToArray());
                return true;
            }

            if (rawClass == "digit")
            {
                for (char c = '0'; c <= '9'; c++)
                    included.Add(c);

                @class = new CharacterClass(included.ToArray());
                return true;
            }

            if (rawClass.StartsWith('[') && rawClass.EndsWith(']'))
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