# C# Password Generator

[![CI Build](https://github.com/blowdart/idunno.PasswordGenerator/actions/workflows/ci-build.yml/badge.svg)](https://github.com/blowdart/idunno.PasswordGenerator/actions/workflows/ci-build.yml)
[![CodeQL](https://github.com/blowdart/idunno.PasswordGenerator/actions/workflows/codeql-analysis.yml/badge.svg)](https://github.com/blowdart/idunno.PasswordGenerator/actions/workflows/codeql-analysis.yml)
[![NuGet Version](https://img.shields.io/nuget/v/idunno.Password.Generator.svg)](https://www.nuget.org/packages/idunno.Password.Generator/)
[![License](https://img.shields.io/github/license/blowdart/idunno.PasswordGenerator.svg)](https://github.com/blowdart/idunno.PasswordGenerator/blob/main/LICENSE)

This library provides a random password generator in C#. The type of algorithm is commonly used when generating website passwords.

Randomness is provided by the [RandomNumberGenerator](https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.randomnumbergenerator) class,
which is a cryptographic random number generator.

This was inspired by a tweet asking how to generate safe passwords in .NET and a Google search which ended up finding 
Seth Vargo's Golang [Password Generator](https://github.com/sethvargo/go-password).

## Usage

```c#
using idunno.Password;

// Generate a password that is 64 characters long with 10 digits, 10 symbols,
// allowing upper and lower case letters, disallowing repeat characters.
var generatedPassword = PasswordGenerator.Generate(64, 10, 10, false, false);

// Generate a password that is 32 characters long with 5 digits, 5 symbols, and a custom set of allowed symbols.
var generatedPassword = PasswordGenerator.Generate(32, 5, 5, symbols: "!@#$%^&*()");
```

## nuget packages

A nuget package is available at https://www.nuget.org/packages/idunno.Password.Generator.

## License

This code is licensed under the Apache license.
