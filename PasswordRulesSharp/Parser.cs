using System.Linq;
using System.Text.RegularExpressions;

namespace PasswordRulesSharp
{
    public class Parser
    {
        const string RegexPattern = @"(?<Name>[\w-]+):\s*(?<Value>[^;]+)\s*;?\s*";

        public (bool Success, int Count) IsValid(string rule)
        {
            var match = Regex.Matches(rule, RegexPattern);
            
            if (!match.Any())
                return (false, 0);

            return (true, match.Count);
        }
    }
}
