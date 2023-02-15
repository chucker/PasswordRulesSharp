using NodaTime;

using PasswordRulesSharp.Models;

using System.Collections.Generic;

namespace PasswordRulesSharp.Rules;

public class Rule : IRule
{
    public static IRule FromString(string rawRule) => new StringRule(rawRule);

    // TODO: convert protected set to init-only

    /// <inheritdoc />
    public int? MinLength { get; protected set; }

    /// <inheritdoc />
    public int? MaxLength { get; protected set; }

    /// <inheritdoc />
    public int? MaxConsecutive { get; protected set; }

    /// <inheritdoc />
    public Period? ExpiresAfter { get; protected set; }

    /// <inheritdoc />
    public List<CharacterClass>? Required { get; protected set; }
}
