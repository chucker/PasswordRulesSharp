using System.Collections.Generic;
using NodaTime;
using PasswordRulesSharp.Models;

namespace PasswordRulesSharp.Rules;

public class IncompleteRule : IRule
{
    /// <inheritdoc />
    public int? MinLength { get; set; }

    /// <inheritdoc />
    public int? MaxLength { get; set; }

    /// <inheritdoc />
    public int? MaxConsecutive { get; set; }

    /// <inheritdoc />
    public Period? ExpiresAfter { get; set; }

    /// <inheritdoc />
    public List<CharacterClass>? Required { get; set; }
}
