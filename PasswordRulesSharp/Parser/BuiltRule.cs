using System.Collections.Generic;
using NodaTime;
using PasswordRulesSharp.Models;

namespace PasswordRulesSharp.Parser
{
    public class BuiltRule : BaseRule
    {
        public BuiltRule(int? minLength, int? maxLength, int? maxConsecutive, Period? expiresAfter, List<CharacterClass>? required)
        {
            MinLength = minLength;
            MaxLength = maxLength;
            MaxConsecutive = maxConsecutive;
            ExpiresAfter = expiresAfter;
            Required = required;
        }
    }
}
