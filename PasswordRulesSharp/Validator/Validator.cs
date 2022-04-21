using PasswordRulesSharp.Parser;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordRulesSharp.Validator
{
    public class Validator
    {
        public Rule Rule { get; }

        public Validator(Rule rule) => Rule = rule;

        public bool PasswordIsValid(string password, out Requirement[] failedRequirements)
        {
            var req = new List<Requirement>();

            if (Rule.MinLength.HasValue && password.Length < Rule.MinLength)
                req.Add(Requirement.MinimumLength);

            if (Rule.MaxLength.HasValue && password.Length > Rule.MaxLength)
                req.Add(Requirement.MaximumLength);

            if (Rule.MaxConsecutive.HasValue)
            {
                int consecutiveCount = 1;

                for (int i = 0; i < password.Length; i++)
                {
                    if (i + 1 < password.Length && password[i] == password[i + 1])
                        consecutiveCount++;
                    else
                        consecutiveCount = 1;

                    if (consecutiveCount > Rule.MaxConsecutive)
                        req.Add(Requirement.MaxConsecutive);
                }
            }

            failedRequirements = req.ToArray();

            return !req.Any();
        }
    }
}
