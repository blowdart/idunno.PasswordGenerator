# C# Password Generator

[![Build Status](https://dev.azure.com/idunno-org/idunno.Password/_apis/build/status/blowdart.idunno.PasswordGenerator?branchName=main)](https://dev.azure.com/idunno-org/idunno.Password/_build/latest?definitionId=2&branchName=main)

This library is a password generator which implements generation of random passwords 
with provided requirements as described by  [AgileBits 1Password](https://discussions.agilebits.com/discussion/23842/how-random-are-the-generated-passwords)
in C#. The algorithm is commonly used when generating website passwords.

Randomness is provided by the [RandomNumberGenerator](https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.randomnumbergenerator?view=net-6.0) class,
which is a cryptographic random number generator.

This was inspired by a tweet asking how to generate safe passwords in .NET and a Google search which ended up finding 
Seth Vargo's Golang [Password Generator](https://github.com/sethvargo/go-password).

## Usage

```c#
using idunno.Password;

// Generate a password that is 64 characters long with 10 digits, 10 symbols,
// allowing upper and lower case letters, disallowing repeat characters.
var passwordGenerator = new PasswordGenerator();
var generatedPassword = passwordGenerator.Generate(64, 10, 10, false, false);
```

## nuget packages

A nuget package is available at https://www.nuget.org/packages/idunno.Password.Generator.

## License

This code is licensed under the MIT license.