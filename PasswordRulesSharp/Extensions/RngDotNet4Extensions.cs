using System;
using System.Security.Cryptography;

namespace PasswordRulesSharp.Extensions;

public static class RngDotNet4Extensions
{
    /// <summary>
    /// .NET 4-compatible alternative to GetInt32
    /// </summary>
    public static int GetInt32(this RandomNumberGenerator rng, int max)
    {
        var bytes = new byte[4];
        rng.GetBytes(bytes);

        uint result;

        result = BitConverter.ToUInt32(bytes, 0);

        return (int)(result % max);
    }
}
