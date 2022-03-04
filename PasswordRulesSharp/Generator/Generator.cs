using PasswordRulesSharp.Parser;

using System;
using System.Linq;
using System.Text;

namespace PasswordRulesSharp.Generator
{
    public class Generator
    {
        public Rule Rule { get; }

        public Generator(Rule rule)
        {
            Rule = rule;
        }

        internal int ChooseLength()
        {
            int length;
            
            // default to 20 chars, unless the minimum is higher...
            if (Rule.MinLength.HasValue && Rule.MinLength >= 20)
                length = Rule.MinLength.Value;
            else
                length = 20;

            // ...or the maximum is lower
            if (Rule.MaxLength.HasValue && Rule.MaxLength < length)
                length = Rule.MaxLength.Value;

            return length;
        }

        internal char[] ChooseChars()
        {
            // if the rule explicitly sets allowed chars, start with those
            //if (Rule.Allowed) // not implemented

            // otherwise, create our own default set
            var defaultSet = CharacterClass.Lower.Included
                             .Union(CharacterClass.Upper.Included)
                             .Union(CharacterClass.Digit.Included);

            return defaultSet.ToArray();
        }

        public string GeneratePassword()
        {
            var length = ChooseLength();

            var chars = ChooseChars();

            var sb = new StringBuilder();
            var random = new Random();

            for (int i = 0; i < length; i++)
            {
                char randomChar = chars[random.Next(chars.Length)];
                sb.Append(randomChar);
            }

            return sb.ToString();
        }
    }
}
