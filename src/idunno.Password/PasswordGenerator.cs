// Copyright (c) Barry Dorrans. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Security.Cryptography;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace idunno.Password;
#pragma warning restore IDE0130 // Namespace does not match folder structure

/// <summary>
/// A utility class for the generation of random passwords with provided requirements once described in a AgileBits/1Password forum post.
/// </summary>
public static class PasswordGenerator
{
    private const int StackAllocationThreshold = 256;

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

        char[]? rented = length > StackAllocationThreshold ? new char[length] : null;
        Span<char> buffer = rented ?? stackalloc char[length];

        // When repeats are disallowed a single set tracks every character already placed so that
        // the whole password stays free of duplicates across all three character categories.
        HashSet<char>? used = allowRepeatedCharacters ? null : new HashSet<char>(length);

        int position = 0;
        AppendRandomCharacters(letters, charactersInPassword, buffer, ref position, used, nameof(length));
        AppendRandomCharacters(digits, numberOfDigits, buffer, ref position, used, nameof(numberOfDigits));
        AppendRandomCharacters(symbols, numberOfSymbols, buffer, ref position, used, nameof(numberOfSymbols));

        Shuffle(buffer[..position]);

        return new string(buffer[..position]);
    }

    /// <summary>
    /// Appends <paramref name="quantity"/> randomly selected characters from <paramref name="pool"/> to
    /// <paramref name="buffer"/>. When <paramref name="used"/> is supplied the selected characters are
    /// distinct, are not already present in <paramref name="used"/>, and are added to it.
    /// </summary>
    private static void AppendRandomCharacters(string pool, int quantity, Span<char> buffer, ref int position, HashSet<char>? used, string parameterName)
    {
        if (quantity <= 0)
        {
            return;
        }

        if (used is null)
        {
            for (int i = 0; i < quantity; i++)
            {
                buffer[position++] = pool[RandomNumberGenerator.GetInt32(0, pool.Length)];
            }

            return;
        }

        // Collect the distinct characters that have not already been placed by another category, then
        // select the required number of them without replacement. Drawing without replacement removes
        // the draw and retry loop the previous implementation relied on.
        Span<char> candidates = pool.Length <= StackAllocationThreshold ? stackalloc char[pool.Length] : new char[pool.Length];
        int candidateCount = 0;

        foreach (char candidate in pool)
        {
            if (!used.Contains(candidate) && candidates[..candidateCount].IndexOf(candidate) < 0)
            {
                candidates[candidateCount++] = candidate;
            }
        }

        if (candidateCount < quantity)
        {
            throw new ArgumentOutOfRangeException(parameterName, "Number of characters requested exceeds the available distinct characters and repeats are not allowed.");
        }

        for (int i = 0; i < quantity; i++)
        {
            int selectedIndex = i + RandomNumberGenerator.GetInt32(0, candidateCount - i);
            char selected = candidates[selectedIndex];
            candidates[selectedIndex] = candidates[i];

            buffer[position++] = selected;
            used.Add(selected);
        }
    }

    /// <summary>
    /// Randomly permutes <paramref name="characters"/> in place using a cryptographically secure shuffle.
    /// </summary>
    private static void Shuffle(Span<char> characters)
    {
        for (int i = characters.Length - 1; i > 0; i--)
        {
            int swapIndex = RandomNumberGenerator.GetInt32(0, i + 1);
            (characters[i], characters[swapIndex]) = (characters[swapIndex], characters[i]);
        }
    }
}