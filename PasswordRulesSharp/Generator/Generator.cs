using PasswordRulesSharp.Models;
using PasswordRulesSharp.Parser;

using System;
using System.Collections.Generic;
using System.Text;
using PasswordRulesSharp.Rules;
using Toore.Shuffling;

namespace PasswordRulesSharp.Generator
{
    public class Generator
    {
        public IRule Rule { get; }

        public Generator(IRule rule)
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
            // TODO: if Rule.Allowed isn't set, default to AsciiPrintable

            // otherwise, create our own default set
            resultSet.Add(CharacterClass.Lower.Chars);
            resultSet.Add(CharacterClass.Upper.Chars);
            resultSet.Add(CharacterClass.Digit.Chars);

            // if the rule contains required chars, make sure those are in the set
            if (Rule.Required != null)
            {
                foreach (var item in Rule.Required)
                {
                    // FIXME: how do we handle unicode here?

                    resultSet.Add(((SpecificCharacterClass)item).Chars);
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

                foreach (var currentCharset in chars)
                {
                    // just pick any char within the selection
                    char randomChar = currentCharset[random.Next(currentCharset.Length)];

                    if (Rule.MaxConsecutive.HasValue)
                    {
                        while (HasConsecutiveChar(ref randomChar, ref sb))
                        {
                            randomChar = currentCharset[random.Next(currentCharset.Length)];
                        }
                    }

                    sb.Append(randomChar);

                    i++;

                    if (i == length)
                        break;
                }
            }

            return sb.ToString();
        }

        private bool HasConsecutiveChar(ref char randomChar, ref StringBuilder sb)
        {
            string existingChars = sb.ToString();

            int consecutiveCount = 0;

            for (int i = existingChars.Length; i < Rule.MaxConsecutive; i--)
            {
                if (i + 1 > existingChars.Length)
                    continue;

                if (i < Rule.MaxConsecutive)
                    break;

                if (existingChars[i + 1] == randomChar)
                    consecutiveCount++;
                else
                    consecutiveCount = 0;
            }

            if (consecutiveCount > Rule.MaxConsecutive)
                return true;

            return false;
        }
    }
}
