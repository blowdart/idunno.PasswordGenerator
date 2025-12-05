# C# Password Generator

This library is a password generator which implements generation of random passwords 
with provided requirements as described by  [AgileBits 1Password](https://discussions.agilebits.com/discussion/23842/how-random-are-the-generated-passwords)
in C#. The algorithm is commonly used when generating website passwords.

Randomness is provided by the [RandomNumberGenerator](https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.randomnumbergenerator) class,
which is a cryptographic random number generator.

## Usage

```c#
using idunno.Password;

// Generate a password that is 64 characters long with 10 digits, 10 symbols,
// allowing upper and lower case letters, disallowing repeat characters.
var passwordGenerator = new PasswordGenerator();
var generatedPassword = passwordGenerator.Generate(64, 10, 10, false, false);
```

## License

This code is licensed under the Apache license.