using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PasswordRulesSharp
{
    public class Parser
    {
        const string RegexPattern = @"(?<Name>[\w-]+):\s*(?<Value>[^;]+);";

        public bool IsValid(string rule)
        {
            var match = System.Text.RegularExpressions.Regex.Match(rule, RegexPattern);

            return true;
        }
    }
}
