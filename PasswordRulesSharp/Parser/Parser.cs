using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace PasswordRulesSharp.Parser
{
    public class Parser
    {
        const string RegexPattern = @"(?<Name>[\w-]+):\s*(?<Value>[^;]+)\s*;?\s*";

        public Dictionary<string, List<string>> GetKeyValuePairs(string rule)
        {
            var matches = Regex.Matches(rule, RegexPattern);

            return matches.OfType<Match>()
                          .GroupBy(k => k.Groups["Name"].Value)
                          .ToDictionary(v => v.Key, m => m.Select(x => x.Groups["Value"].Value).ToList());
        }

        public (bool Success, int Count) IsValid(string rule)
        {
            var matches = Regex.Matches(rule, RegexPattern);
            
            if (!matches.OfType<Match>().Any())
                return (false, 0);

            return (true, matches.Count);
        }
    }
}
