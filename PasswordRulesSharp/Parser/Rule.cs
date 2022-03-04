using System.Collections.Generic;
using System.Linq;

namespace PasswordRulesSharp.Parser
{
    public class Rule
    {
        public enum Requirement
        {
            MinimumLength,
            MaximumLength
        }

        public int? MinLength { get; }
        public int? MaxLength { get; }

        public CharacterClass? Required { get; }

        public Rule(string rule)
        {
            var dict = new Parser().GetKeyValuePairs(rule);

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

            // TODO: this isn't correct. we need multiple required rules, and maybe AND-combine them?
            if (dict.TryGetValue("required", out value) &&
                CharacterClass.TryParse(value[0], out var required))
            {
                Required = required;
            }
            else
            {
                // TODO: fallback?
            }

            // TODO: and then for allowed rules, OR-combine them?
        }

        public bool PasswordIsValid(string password, out Requirement[] failedRequirements)
        {
            var req = new List<Requirement>();

            if (MinLength.HasValue && password.Length < MinLength)
                req.Add(Requirement.MinimumLength);

            if (MaxLength.HasValue && password.Length > MaxLength)
                req.Add(Requirement.MaximumLength);

            failedRequirements = req.ToArray();

            return !req.Any();
        }
    }
}
