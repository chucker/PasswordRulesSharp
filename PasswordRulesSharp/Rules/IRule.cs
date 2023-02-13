using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NodaTime;
using PasswordRulesSharp.Models;

namespace PasswordRulesSharp.Rules;

public interface IRule
{
    /// <summary>
    /// The minimum length of a valid password, in chars.
    /// </summary>
    public int? MinLength { get; }

    /// <summary>
    /// <para>
    /// The maximum length of a valid password, in chars.
    /// </para>
    /// 
    /// <para>
    /// If <see cref="MinLength"/> is also set, must be greater or equal.
    /// </para>
    /// </summary>
    public int? MaxLength { get; }

    /// <summary>
    /// <para>
    /// Maximum consecutive chars.
    /// </para>
    /// 
    /// <para>
    /// Defaults to unlimited. Must be positive. If you set it to e.g. 3,
    /// the password 'aaaa' is not valid.
    /// </para>
    /// </summary>
    [Range(1, int.MaxValue)]
    public int? MaxConsecutive { get; }

    /// <summary>
    /// Password expires after a period of time (e.g., 3 months). This is a
    /// non-standard extension.
    /// </summary>
    public Period? ExpiresAfter { get; }

    /// <summary>
    /// <para>
    /// A list of required character classes.
    /// </para>
    /// 
    /// <para>
    /// Each of these is at-least-one-of. For example, <c>required: lower;
    /// required: upper;</c> means at least one lower-case _and_ at least
    /// one upper-case character is required. <c>required: [!@]</c> means
    /// _one_ of those two special chars is required, whereas
    /// <c>required: [!]; required: [@];</c> means _both_ are required.
    /// </para>
    /// </summary>
    public List<CharacterClass>? Required { get; }
}
