using PasswordRulesSharp.Parser;

using System;
using System.Collections.Generic;
using System.Text;

using Toore.Shuffling;

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

        internal char[][] ChooseChars()
        {
            var resultSet = new List<char[]>();

            // if the rule explicitly sets allowed chars, start with those
            //if (Rule.Allowed) // not implemented

            // otherwise, create our own default set
            resultSet.Add(CharacterClass.Lower.Included);
            resultSet.Add(CharacterClass.Upper.Included);
            resultSet.Add(CharacterClass.Digit.Included);

            // if the rule contains required chars, make sure those are in the set
            if (Rule.Required != null)
            {
                foreach (var item in Rule.Required)
                {
                    resultSet.Add(item.Included);
                }
            }

            return resultSet.ToArray();
        }

        public string GeneratePassword()
        {
            var length = ChooseLength();

            var chars = ChooseChars();

            var sb = new StringBuilder();
            var random = new Random();

            var shuffler = new FisherYatesShuffler(new RandomWrapper());

            int i = 0;
            while (i < length)
            {
                // ensure each selection of chars is used, but in a random order
                chars = chars.Shuffle(shuffler).ToArray();

                foreach (var item in chars)
                {
                    // just pick any char within the selection
                    char randomChar = item[random.Next(item.Length)];
                    sb.Append(randomChar);

                    i++;

                    if (i == length)
                        break;
                }
            }

            return sb.ToString();
        }
    }
}
