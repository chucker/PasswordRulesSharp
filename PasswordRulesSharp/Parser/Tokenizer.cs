using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace PasswordRulesSharp.Parser
{
    public class Tokenizer
    {
        //lang=regex
        const string RegexPattern = @"(?<Name>[\w-]+):\s*(?<Value>[^;]+)\s*;?\s*";

        public string RawRule { get; }

        public Tokenizer(string rawRule)
        {
            RawRule = rawRule;
        }

        public Dictionary<string, List<string>> GetKeyValuePairs()
        {
            var matches = Regex.Matches(RawRule, RegexPattern);

            return matches.OfType<Match>()
                          .GroupBy(k => k.Groups["Name"].Value)
                          .ToDictionary(v => v.Key, m => m.Select(x => x.Groups["Value"].Value).ToList());
        }

        public (bool Success, int Count) IsValid()
        {
            var matches = Regex.Matches(RawRule, RegexPattern);

            if (!matches.OfType<Match>().Any())
                return (false, 0);

            return (true, matches.Count);
        }
    }
}
