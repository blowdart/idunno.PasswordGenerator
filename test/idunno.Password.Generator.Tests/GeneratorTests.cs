// Copyright (c) Barry Dorrans. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace idunno.Password.Generator.Tests
{
    [ExcludeFromCodeCoverage]
    public class GeneratorTests
    {
        [Fact]
        public void SettingLengthReturnsAppropriatePasswordLength()
        {
            var random = new Random();
            var expected = random.Next(1, 25);

            var generator = new PasswordGenerator();

            var result = generator.Generate(expected, 0, 0);

            Assert.Equal(expected, result.Length);
        }

        [Fact]
        public void RequiringADigitIncludesADigit()
        {
            var generator = new PasswordGenerator();

            var result = generator.Generate(10, 1, 0, false, false, "abcdefghij", "ABCDEFGHIJ", "0", "");

            Assert.Contains("0", result);
        }

        [Fact]
        public void RequiringASymbolIncludesASymbol()
        {
            var generator = new PasswordGenerator();

            var result = generator.Generate(10, 0, 1, false, false, "abcdefghij", "ABCDEFGHIJ", "0", "!");

            Assert.Contains("!", result);
        }

        [Fact]
        public void RequiringNoRepeatedCharactersDoesNotGenerateAPasswordWithRepeatedCharacters()
        {
            var generator = new PasswordGenerator();
            var result = generator.Generate(1, 0, 0, false, true, "a", "A", "0", "!");

            Assert.True(result == "A" || result == "a");
        }

        [Fact]
        public void ExcludingUpperCaseGeneratesAPasswordWithoutUpperCase()
        {
            var generator = new PasswordGenerator();
            var result = generator.Generate(1, 0, 0, true, true, "a", "A", "0", "!");

            Assert.Equal("a", result);
        }

        [Fact]
        public void ExcludingDigitsDoesNotIncludeADigit()
        {
            var generator = new PasswordGenerator();

            var result = generator.Generate(10, 0, 1, false, false, "abcdefghij", "ABCDEFGHIJ", "0", "!");

            Assert.DoesNotContain("0", result);
        }

        [Fact]
        public void ExcludingSymbolDoesNotIncludeASymbol()
        {
            var generator = new PasswordGenerator();

            var result = generator.Generate(10, 1, 0, false, false, "abcdefghij", "ABCDEFGHIJ", "0", "!");

            Assert.DoesNotContain("!", result);
        }

        [Fact]
        public void SettingRequiredNumberOfDigitsAndSymbolsGreaterThanRequiredLengthThrows()
        {
            var generator = new PasswordGenerator();

            void action() => generator.Generate(10, 10, 10);

            var exception =  Assert.Throws<ArgumentException>(action);
            Assert.Equal("length", exception.ParamName);
        }

        [Fact]
        public void NotAllowingRepeatedCharactersAndRequiringALengthGreaterThanAvailableCharactersThrows()
        {
            var generator = new PasswordGenerator();

            void action() => generator.Generate(10, 0, 0, true, false, "abcd", "ABCD", "1", "!");

            var exception = Assert.Throws<ArgumentException>(action);
            Assert.Equal("length", exception.ParamName);
        }

        [Fact]
        public void RequiringAMinimumNumberOfDigitsNotAllowingRepeatedCharactersAndRequiringMoreDigitsThanProvidedThrows()
        {
            var generator = new PasswordGenerator();

            void action() => generator.Generate(4, 2, 2, true, false, "abc", "ABC", "1", "!@#");

            var exception = Assert.Throws<ArgumentException>(action);
            Assert.Equal("numberOfDigits", exception.ParamName);
        }

        [Fact]
        public void RequiringAMinimumNumberOfSymbolsNotAllowingRepeatedCharactersAndRequiringMoreSymbolsThanProvidedThrows()
        {
            var generator = new PasswordGenerator();

            void action() => generator.Generate(4, 2, 2, true, false, "abc", "ABC", "123", "!");

            var exception = Assert.Throws<ArgumentException>(action);
            Assert.Equal("numberOfSymbols", exception.ParamName);
        }
    }
}