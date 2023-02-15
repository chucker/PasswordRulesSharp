using System.Collections.Generic;
using NodaTime;
using PasswordRulesSharp.Models;

namespace PasswordRulesSharp.Rules
{
    public class BuiltRule : Rule
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
