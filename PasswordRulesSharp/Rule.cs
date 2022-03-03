using System.Collections.Generic;

namespace PasswordRulesSharp
{
    public class Rule
    {
        public int? MinLength { get; }
        public int? MaxLength { get; }

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
        }
    }
}
