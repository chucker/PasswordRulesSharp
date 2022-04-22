using System.Diagnostics;

namespace PasswordRulesSharp.Models
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class SpecificCharacterClass : CharacterClass
    {
        public string DebuggerDisplay => $"[{string.Join(", ", Chars)}]";

        public char[] Chars { get; }

        internal SpecificCharacterClass(char[] chars)
        {
            Chars = chars;
        }
    }
}
