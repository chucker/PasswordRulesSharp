using PasswordRulesSharp.Models;

namespace PasswordRulesSharp.Validator.Requirements
{
    public abstract class Requirement
    {
        public RequirementType Type { get; }

        public Requirement(RequirementType type) => Type = type;
    }

    public class CharacterClassRequirement : Requirement
    {
        public CharacterClass CharacterClass { get; }

        public CharacterClassRequirement(CharacterClass characterClass)
            : base(RequirementType.RequiredChars)
        {
            CharacterClass = characterClass;
        }
    }

    public class MinimumLengthRequirement : Requirement
    {
        public int Length { get; }

        public MinimumLengthRequirement(int length)
            : base(RequirementType.MinimumLength)
        {
            Length = length;
        }
    }

    public class MaximumLengthRequirement : Requirement
    {
        public int Length { get; }

        public MaximumLengthRequirement(int length)
            : base(RequirementType.MaximumLength)
        {
            Length = length;
        }
    }

    public class MaxConsecutiveRequirement : Requirement
    {
        public int Value { get; }

        public MaxConsecutiveRequirement(int value)
            : base(RequirementType.MaxConsecutive)
        {
            Value = value;
        }
    }
}
