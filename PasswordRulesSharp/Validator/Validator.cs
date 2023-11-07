using PasswordRulesSharp.Models;
using PasswordRulesSharp.Rules;
using PasswordRulesSharp.Validator.Requirements;

using System.Collections.Generic;
using System.Linq;

namespace PasswordRulesSharp.Validator
{
    /// <summary>
    /// A service that matches a rule against a provided password, and tells
    /// you whether the requirements are met.
    /// </summary>
    public class Validator
    {
        /// <summary>
        /// The rule to match against.
        /// </summary>
        public IRule Rule { get; }

        /// <summary>
        /// Creates a validator based on a given rule.
        /// </summary>
        public Validator(IRule rule) => Rule = rule;

        /// <summary>
        /// Ascertains whether <paramref name="password"/> is "valid", meaning
        /// it meets all requirements in the rule. If not,
        /// <paramref name="requirementStatuses"/> contains detailed
        /// information on which requirements have failed.
        /// </summary>
        public bool PasswordIsValid(string password, out (Requirement Requirement, bool Success)[] requirementStatuses)
        {
            var req = new List<(Requirement Requirement, bool Success)>();

            if (Rule.Required != null)
            {
                foreach (var item in Rule.Required)
                {
                    switch (item)
                    {
                        case SpecificCharacterClass specific:
                            req.Add((new CharacterClassRequirement(item), password.Any(c => specific.Chars.Contains(c))));
                            break;

                        case UnicodeCharacterClass unicode:
                            // for Unicode, *any* character will do
                            req.Add((new CharacterClassRequirement(item), password.Any()));
                            break;
                    }
                }
            }

            if (Rule.MinLength.HasValue)
                req.Add((new MinimumLengthRequirement(Rule.MinLength.Value), password.Length >= Rule.MinLength));

            if (Rule.MaxLength.HasValue)
                req.Add((new MaximumLengthRequirement(Rule.MaxLength.Value), password.Length <= Rule.MaxLength));

            if (Rule.MaxConsecutive.HasValue)
            {
                bool success = true;
                int consecutiveCount = 1;

                for (int i = 0; i < password.Length; i++)
                {
                    if (i + 1 < password.Length && password[i] == password[i + 1])
                        consecutiveCount++;
                    else
                        consecutiveCount = 1;

                    if (consecutiveCount > Rule.MaxConsecutive)
                        success = false;
                }

                req.Add((new MaxConsecutiveRequirement(Rule.MaxConsecutive.Value), success));
            }

            requirementStatuses = req.ToArray();

            return req.All(rs => rs.Success);
        }
    }
}
