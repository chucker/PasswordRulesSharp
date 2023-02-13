const string rawRule = "minlength: 8; maxlength: 10; required: lower; required: upper; required: digit; required: [-]";

var rule = new PasswordRulesSharp.Parser.Rule(rawRule);

const string samplePassword1 = "Hello2AndSomeMoreText-";

var validator = new PasswordRulesSharp.Validator.Validator(rule);

bool password1IsValid = validator.PasswordIsValid(samplePassword1, out var results1);

Console.ReadLine();

const string samplePassword2 = "Hello1";

bool password2IsValid = validator.PasswordIsValid(samplePassword2, out _);

Console.ReadLine();

var generator = new PasswordRulesSharp.Generator.Generator(rule);

var newPassword = generator.GeneratePassword();

for (int i = 0; i < 5; i++)
{
    Console.WriteLine(generator.GeneratePassword());
}

Console.ReadLine();
