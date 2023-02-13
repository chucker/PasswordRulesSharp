using NodaTime;

using PasswordRulesSharp.Models;
using PasswordRulesSharp.Rules.Parsing;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;

namespace PasswordRulesSharp.Parser
{
    public class StringRule : BaseRule
    {
        public StringRule(string rawRule)
        {
            var dict = new Tokenizer(rawRule).GetKeyValuePairs();

            List<string>? value;

            if (dict.TryGetValue("minlength", out value) &&
                value.Count == 1 &&
                int.TryParse(value[0], out var minLength))
            {
                MinLength = minLength;
            }
            else
            {
                // TODO set fallback?
            }

            if (dict.TryGetValue("maxlength", out value) &&
                value.Count == 1 &&
                int.TryParse(value[0], out var maxLength))
            {
                MaxLength = maxLength;

                if (MaxLength < 4)
                    MaxLength = 4; // https://developer.apple.com/password-rules/ rejects lengths < 4

                if (MaxLength < MinLength)
                    MinLength = MaxLength;
            }

            if (dict.TryGetValue("max-consecutive", out value))
            {
                // "If you have multiple max-consecutive properties in your rule, the minimum value of the properties will be applied."
                var maxConsecutive = value.Where(s => int.TryParse(s, out var intVal))
                                          .Select(s => int.Parse(s))
                                          .Min();
                MaxConsecutive = maxConsecutive;
            }

            // TODO: this isn't correct. we need multiple required rules, and maybe AND-combine them?
            if (dict.TryGetValue("required", out value))
            {
                Required = new();

                foreach (var item in value)
                {
                    if (CharacterClass.TryParse(item, out var required))
                    {
                        Required.Add(required);
                    }
                }
            }
            else
            {
                // TODO: fallback?
            }

            // TODO: and then for allowed rules, OR-combine them?

            if (dict.TryGetValue("x-expires-after", out value) &&
                value.Count == 1 &&
                TryParsePeriod(value[0], out var periodVal))
            {
                ExpiresAfter = periodVal;
            }
        }

        private static bool TryParsePeriod(string s, [NotNullWhen(true)] out Period? periodVal)
        {
            periodVal = null;

            //lang=regex
            const string RegexPattern = @"(?<Amount>\d+)-(?<Unit>(days|weeks|months|years))";

            var match = Regex.Match(s, RegexPattern);

            foreach (var item in new[] { "Amount", "Unit" })
            {
                if (!match.Groups[item].Success)
                    return false;
            }

            var amount = int.Parse(match.Groups["Amount"].Value);

            periodVal = match.Groups["Unit"].Value switch
            {
                "days" => Period.FromDays(amount),
                "weeks" => Period.FromWeeks(amount),
                "months" => Period.FromMonths(amount),
                "years" => Period.FromYears(amount),
                _ => throw new ArgumentOutOfRangeException(s),
            };

            return true;
        }
    }
}
