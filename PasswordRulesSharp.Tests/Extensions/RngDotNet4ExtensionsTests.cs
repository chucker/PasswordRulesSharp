using NUnit.Framework;

using System.Security.Cryptography;

namespace PasswordRulesSharp.Extensions.Tests;

public class RngDotNet4ExtensionsTests
{
    public RandomNumberGenerator Rng { get; private set; }

    [SetUp]
    public void SetUp()
    {
        Rng = RandomNumberGenerator.Create();
    }

    [TestCase(1)]
    [TestCase(10)]
    [TestCase(100)]
    public void GetIndex(int max)
    {
        Assert.That(Rng.GetInt32(max), Is.InRange(0, max));
    }
}
