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

            failedRequirements = req.ToArray();

            return !req.Any();
        }
    }
}
