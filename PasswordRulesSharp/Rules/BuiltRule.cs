using NodaTime;

using PasswordRulesSharp.Models;

using System.Collections.Generic;

namespace PasswordRulesSharp.Rules
{
    internal class BuiltRule : Rule
    {
        internal BuiltRule(int? minLength, int? maxLength, int? maxConsecutive,
                           Period? expiresAfter, List<CharacterClass>? required)
        {
            MinLength = minLength;
            MaxLength = maxLength;
            MaxConsecutive = maxConsecutive;
            ExpiresAfter = expiresAfter;
            Required = required;
        }
    }
}
