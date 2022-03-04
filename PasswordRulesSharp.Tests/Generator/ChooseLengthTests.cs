using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordRulesSharp.Tests.Generator
{
    public class ChooseLengthTests
    {
        [TestCase("minlength: 20; required: lower; required: upper; required: digit; required: [-];", 20)]
        [TestCase("minlength: 8", 20)]
        [TestCase("minlength: 20; required: lower; required: upper; required: digit; required: [-];", 20)]
        [TestCase("minlength: 20; maxlength: 30; required: lower; required: upper; required: digit; required: [-];", 20)]
        [TestCase("minlength: 8; maxlength: 7", 7)]
        [TestCase("minlength: 8 ; maxlength: 3", 4)]
        public void ChooseLength(string rule, int expectedLength)
        {
            var parsedRule = new PasswordRulesSharp.Parser.Rule(rule);

            Assert.AreEqual(expectedLength, parsedRule.MinLength);
        }
    }
}
