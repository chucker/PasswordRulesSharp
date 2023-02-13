# PasswordRulesSharp
A C# implementation of an Apple-compatible Password Rules syntax

## Usage

First, pick a rule in raw syntax. You can use [Apple's tool](https://developer.apple.com/password-rules/) to help
create a rule. For example, let's use `minlength: 20; required: lower; required: upper; required: digit; required: [-];`.
This requires that the password is at least 20 characters wrong, has at least one lower- and uppercase character each,
has a digit, and has a hyphen (`-`);

### Validation

Instantiate this as a rule:

```csharp
const rawRule = "minlength: 20; required: lower; required: upper; required: digit; required: [-]";

var rule = new PasswordRulesSharp.Parser.Rule(rawRule);
```

Now, you can take a password and validate whether it matches that rule:

```csharp
const string samplePassword1 = "Hello2AndSomeMoreText-";

var validator = new PasswordRulesSharp.Validator.Validator(rule);

bool password1IsValid = validator.PasswordIsValid(samplePassword1, out var results1);
```

This returns `true`, as the password matches all rules. Let's try with one that doesn't:

```csharp
const string samplePassword2 = "Hello1";

bool password2IsValid = validator.PasswordIsValid(samplePassword2, out var results2);
```

This returns `false`, of course. We can look at `results2` as to why that is: the library found five requirements. Four
are `CharacterClassRequirement`s, and the fifth is a `MinimumLengthRequirement`. We fulfill three of the character
class requirements, but we don't have a hyphen, so we fail that one. We also don't meet the minimum length.

You can also discard the detailed results if you prefer:

```csharp
bool password2IsValid = validator.PasswordIsValid(samplePassword2, out _);
```

This is still enough if you just want a yes/no "is it valid" answer.

### Generation

This flow is enough if your users want to pick passwords themselves. But you may also prefer for passwords to be
_generated_. For that, instantiate the `Generator`:

```csharp
var generator = new PasswordRulesSharp.Generator.Generator(rule);

var newPassword = generator.GeneratePassword();
```

You may also find it useful to generate a handful of passwords:

```csharp
for (int i = 0; i < 5; i++)
{
    Console.WriteLine(generator.GeneratePassword());
}
```

The output will look like:

    -0fBAp4q24d-VA9tO2wC
    -nA7A6q-o9S4Jo-1SkD1
    80piB-N4Mv8q-AxG0-o8
    -hY0i6T-5UcI6jNQ7q-f
    b-a7B6C-2FuO8rZyv4R-

All generated passwords will match the rules we've specified. As you can see,
they're all 20 characters long, and each contain at least one hyphen, one
digit, lower-case character, and upper-case character.

#### Of note:

* If you pass a `minlength` _below_ `20`, the generated passwords will still be
at least 20 characters long, for security reasons. If you _want_ to generate a
shorter password, you can achieve that by also specifying a `maxlength`. For
example, `minlength: 8; maxlength: 10; required: lower; required: upper; required: digit; required: [-]`
will yield generated passwords that are 10 characters long.

* As you can see, there is no logic for the generated passwords to
follow a particular pattern. This increases entropy (and therefore security),
but "looks" slightly less nice.

## References

Apple's docs: https://developer.apple.com/documentation/security/password_autofill/customizing_password_autofill_rules

Apple's validation tool: https://developer.apple.com/password-rules/

A Rust port: https://docs.rs/password-rules-parser/latest/password_rules_parser/

### Further reading

There's also https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.identity.passwordoptions, but it is far more limiting. (Someone might write an importer from that format.)
