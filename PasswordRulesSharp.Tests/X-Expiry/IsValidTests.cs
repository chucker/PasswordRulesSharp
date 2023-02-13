using NodaTime;

using NUnit.Framework;

using PasswordRulesSharp.Parser;
using PasswordRulesSharp.Rules;
using PasswordRulesSharp.Rules.Parsing;

namespace PasswordRulesSharp.Tests.X_Expiry
{
    public class IsValidTests
    {
        static (string RawRule, bool ExpectedIsValid, Period? ExpectedDuration)[] IsValidCases =
        {
            ("minlength: 20; x-expires-after: ;", false, null),
            ("minlength: 20; x-expires-after: 4-wee;", false, null),
            ("minlength: 20; x-expires-after: 4-weeks;", true, Period.FromWeeks(4)),
            ("minlength: 20; x-expires-after: 3-months;", true, Period.FromMonths(3)),
            ("minlength: 20; x-expires-after: 2-years;", true, Period.FromYears(2))
        };

        [Test]
        [TestCaseSource(nameof(IsValidCases))]
        public void IsValid((string RawRule, bool ExpectedIsValid, Period? ExpectedDuration) input)
        {
            var tokenizer = new Tokenizer(input.RawRule);

            var result = tokenizer.IsValid();

            var rule = new StringRule(input.RawRule);

            if (input.ExpectedIsValid)
            {
                Assert.That(rule.ExpiresAfter, Is.EqualTo(input.ExpectedDuration));
            }
            else
            {
                Assert.That(rule.ExpiresAfter, Is.Null);
            }
        }
    }
}
