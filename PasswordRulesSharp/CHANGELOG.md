# Changelog

## 2.0.0

* In 1.x, rules could be built from a raw string in Apple's syntax. In 2.0, you can also build them in C# with the `RuleBuilder`.
* The generator has been improved: previously, randomness wasn't crytographic.

## 1.2.0

Added support for OR-combining multiple character classes. (Thanks @ricardoboss!)

For example, a requirement of `required: upper,lower` requires at least one upper-case _or_ lower-case character, not
necessarily both.

## 1.1.0

Added preliminary support for `x-expires-after`. This is a proprietary extension that defines a duration after which a
password is no longer valid.

For example, a requirement of `x-expires-after: 3-months;` requires the user to set a new password after every three
months.

