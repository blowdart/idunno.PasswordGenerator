// Copyright (c) Barry Dorrans. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics.CodeAnalysis;

using Xunit;

namespace idunno.Password.Generator.Tests;

[ExcludeFromCodeCoverage]
public class GeneratorTests
{
    [Fact]
    public void SettingLengthReturnsAppropriatePasswordLength()
    {
        var random = new Random();
        var expected = random.Next(1, 25);

        var result = PasswordGenerator.Generate(expected, 0, 0);

        Assert.Equal(expected, result.Length);
    }

    [Fact]
    public void RequiringADigitIncludesADigit()
    {
        var result = PasswordGenerator.Generate(10, 1, 0, false, false, "abcdefghij", "ABCDEFGHIJ", "0", "");

        Assert.Contains("0", result);
    }

    [Fact]
    public void RequiringASymbolIncludesASymbol()
    {
        var result = PasswordGenerator.Generate(10, 0, 1, false, false, "abcdefghij", "ABCDEFGHIJ", "0", "!");

        Assert.Contains("!", result);
    }

    [Fact]
    public void RequiringNoRepeatedCharactersDoesNotGenerateAPasswordWithRepeatedCharacters()
    {
        var result = PasswordGenerator.Generate(1, 0, 0, false, true, "a", "A", "0", "!");

        Assert.True(result == "A" || result == "a");
    }

    [Fact]
    public void ExcludingUpperCaseGeneratesAPasswordWithoutUpperCase()
    {
        var result = PasswordGenerator.Generate(1, 0, 0, true, true, "a", "A", "0", "!");

        Assert.Equal("a", result);
    }

    [Fact]
    public void ExcludingDigitsDoesNotIncludeADigit()
    {
        var result = PasswordGenerator.Generate(10, 0, 1, false, false, "abcdefghij", "ABCDEFGHIJ", "0", "!");

        Assert.DoesNotContain("0", result);
    }

    [Fact]
    public void ExcludingSymbolDoesNotIncludeASymbol()
    {
        var result = PasswordGenerator.Generate(10, 1, 0, false, false, "abcdefghij", "ABCDEFGHIJ", "0", "!");

        Assert.DoesNotContain("!", result);
    }

    [Fact]
    public void SettingRequiredNumberOfDigitsAndSymbolsGreaterThanRequiredLengthThrows()
    {
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            PasswordGenerator.Generate(10, 10, 10));
        Assert.Equal("length", exception.ParamName);
    }

    [Fact]
    public void NotAllowingRepeatedCharactersAndRequiringALengthGreaterThanAvailableCharactersThrows()
    {
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            PasswordGenerator.Generate(10, 0, 0, true, false, "abcd", "ABCD", "1", "!"));
        Assert.Equal("length", exception.ParamName);
    }

    [Fact]
    public void RequiringAMinimumNumberOfDigitsNotAllowingRepeatedCharactersAndRequiringMoreDigitsThanProvidedThrows()
    {
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            PasswordGenerator.Generate(4, 2, 2, true, false, "abc", "ABC", "1", "!@#"));
        Assert.Equal("numberOfDigits", exception.ParamName);
    }

    [Fact]
    public void RequiringAMinimumNumberOfSymbolsNotAllowingRepeatedCharactersAndRequiringMoreSymbolsThanProvidedThrows()
    {
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            PasswordGenerator.Generate(4, 2, 2, true, false, "abc", "ABC", "123", "!"));
        Assert.Equal("numberOfSymbols", exception.ParamName);
    }
}