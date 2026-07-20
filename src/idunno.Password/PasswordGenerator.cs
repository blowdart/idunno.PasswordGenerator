// Copyright (c) Barry Dorrans. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace idunno.Password;
#pragma warning restore IDE0130 // Namespace does not match folder structure

/// <summary>
/// A utility class for the generation of random passwords with provided requirements once described in a AgileBits/1Password forum post.
/// </summary>
public static class PasswordGenerator
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PasswordGenerator"/> class.
    /// </summary>
    static PasswordGenerator()
    {
    }

    /// <summary>
    /// Generates a random password string to the specified requirements.
    /// These types of passwords are typically used for web site passwords.
    /// </summary>
    /// <param name="length">The length of the password to generate.</param>
    /// <param name="numberOfDigits">The number of digits to include in the generated password.</param>
    /// <param name="numberOfSymbols">The number of symbols to include in the generated password.</param>
    /// <param name="noUpperCase">Flag indicating whether no upper case symbols should be included in the generated password.</param>
    /// <param name="allowRepeatedCharacters">Flag indicating whether repeated characters should be allowed in the generated password.</param>
    /// <param name="lowerCaseLetters">A string containing characters used as lower case characters in the password.</param>
    /// <param name="upperCaseLetters">A string containing characters used as upper case characters in the password.</param>
    /// <param name="digits">A string containing characters used as digits in the password.</param>
    /// <param name="symbols">A string containing characters used as symbols in the password.</param>
    /// <returns>A random password string with the requirements specified by the parameters</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the restrictive arguments mean generation cannot be fulfilled.</exception>
    [SuppressMessage("CodeSmell", "S127: \"for\" loop stop conditions should be invariant", Justification = "The loop stop conditions are intentionally variable based on input parameters.")]
    public static string Generate(
        int length,
        int numberOfDigits,
        int numberOfSymbols,
        bool noUpperCase = false,
        bool allowRepeatedCharacters = false,
        string lowerCaseLetters = "abcdefghijklmnopqrstuvwxyz",
        string upperCaseLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ",
        string digits = "0123456789",
        string symbols = "~!@#$%^&*()_+`-={}|[]\\:\"<>?,./")
    {
        if (length <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(length), "Length must be greater than zero.");
        }

        if (numberOfDigits < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(numberOfDigits), "Number of digits must be greater than or equal to zero.");
        }

        if (numberOfSymbols < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(numberOfSymbols), "Number of symbols must be greater than or equal to zero.");
        }

        if (string.IsNullOrWhiteSpace(lowerCaseLetters))
        {
            throw new ArgumentException("Value cannot be null or empty.", nameof(lowerCaseLetters));
        }

        if (string.IsNullOrWhiteSpace(upperCaseLetters) && !noUpperCase)
        {
            throw new ArgumentException("Value cannot be null or empty.", nameof(upperCaseLetters));
        }

        if (string.IsNullOrWhiteSpace(digits) && numberOfDigits > 0)
        {
            throw new ArgumentException("Value cannot be null or empty.", nameof(digits));
        }

        if (string.IsNullOrWhiteSpace(symbols) && numberOfSymbols > 0)
        {
            throw new ArgumentException("Value cannot be null or empty.", nameof(symbols));
        }

        string letters = lowerCaseLetters;

        if (!noUpperCase)
        {
            letters += upperCaseLetters;
        }

        int charactersInPassword = length - numberOfDigits - numberOfSymbols;

        if (charactersInPassword < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(length), "Number of digits and symbols must be less than length.");
        }

        if (!allowRepeatedCharacters && charactersInPassword > letters.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(length), "Number of characters requested exceeds available letters and repeats are not allowed");
        }

        if (!allowRepeatedCharacters && numberOfDigits > 0 && numberOfDigits > digits.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(numberOfDigits), "Number of digits requested exceeds available digits and repeats are not allowed");
        }

        if (!allowRepeatedCharacters && numberOfSymbols > 0 && numberOfSymbols > symbols.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(numberOfSymbols), "Number of symbols requested exceeds available symbols and repeats are not allowed");
        }

        string result = string.Empty;

        for (int i = 0; i < charactersInPassword; i++)
        {
            char character = GetRandomElement(letters);

            if (!allowRepeatedCharacters && result.Contains(character, StringComparison.InvariantCulture))
            {
                i--;
            }
            else
            {
                result = InsertAtRandomPosition(result, character);
            }
        }

        for (int i = 0; i < numberOfDigits; i++)
        {
            char digit = GetRandomElement(digits);

            if (!allowRepeatedCharacters && result.Contains(digit, StringComparison.InvariantCulture))
            {
                i--;
            }
            else
            {
                result = InsertAtRandomPosition(result, digit);
            }
        }

        for (int i = 0; i < numberOfSymbols; i++)
        {
            char symbol = GetRandomElement(symbols);

            if (!allowRepeatedCharacters && result.Contains(symbol, StringComparison.InvariantCulture))
            {
                i--;
            }
            else
            {
                result = InsertAtRandomPosition(result, symbol);
            }
        }

        return result;
    }

    /// <summary>
    /// Inserts <paramref name="characterToInsert"/> into <paramref name="input"/> at a random position.
    /// </summary>
    /// <param name="input">The string <paramref name="characterToInsert"/> should be inserted into.</param>
    /// <param name="characterToInsert">The character to insert into <paramref name="input"/>.</param>
    /// <returns>A string containing <paramref name="characterToInsert"/> into <paramref name="input"/> at a random position.</returns>
    private static string InsertAtRandomPosition(string input, char characterToInsert)
    {
        int position = input.Length == 0 ? 0 : RandomNumberGenerator.GetInt32(0, input.Length);
        return input.Insert(position, characterToInsert.ToString());
    }

    /// <summary>
    /// Gets a random character from <paramref name="input"/>.
    /// </summary>
    /// <param name="input">The string to select a random character from.</param>
    /// <returns>A random character from <paramref name="input"/>.</returns>
    private static char GetRandomElement(string input)
    {
        int position = RandomNumberGenerator.GetInt32(0, input.Length);
        return input[position];
    }
}