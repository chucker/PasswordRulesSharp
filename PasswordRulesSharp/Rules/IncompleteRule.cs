using NodaTime;

using PasswordRulesSharp.Models;

using System.Collections.Generic;

namespace PasswordRulesSharp.Rules;

internal class IncompleteRule : IRule
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
