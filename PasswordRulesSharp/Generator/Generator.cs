using PasswordRulesSharp.Parser;

namespace PasswordRulesSharp.Generator
{
    public class Generator
    {
        public Rule Rule { get; }

        public Generator(Rule rule)
        {
            Rule = rule;
        }

        internal int ChooseLength()
        {
            int length;
            
            // default to 20 chars, unless the minimum is higher...
            if (Rule.MinLength.HasValue && Rule.MinLength >= 20)
                length = Rule.MinLength.Value;
            else
                length = 20;

            // ...or the maximum is lower
            if (Rule.MaxLength.HasValue && Rule.MaxLength < length)
                length = Rule.MaxLength.Value;

            return length;
        }

        public string GeneratePassword()
        {
            var length = ChooseLength();

            return "";
        }
    }
}
