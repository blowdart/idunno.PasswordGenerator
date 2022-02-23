// Copyright (c) Barry Dorrans. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Security.Cryptography;

namespace idunno.Password
{
    public class PasswordGenerator
    {
        private string LowerCaseLetters { get; set; } = "abcdefghijklmnopqrstuvwxyz";
        private string UpperCaseLetters { get; set; } = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private string Digits { get; set; } = "0123456789";
        private string Symbols { get; set; } = "~!@#$%^&*()_+`-={}|[]\\:\"<>?,./";

        /// <summary>
        /// Generates a random password string with the requirements specified, using the approach described by AgileBytes 1Password.
        /// These types of passwords are typically used for web site passwords.
        /// </summary>
        /// <remarks>
        /// The AgileBytes 1Password algorithm is described at https://1password.community/discussion/23842/how-random-are-the-generated-passwords.
        /// </remarks>
        /// <param name="length">The length of the password to generate.</param>
        /// <param name="numberOfDigits">The number of digits to include in the generated password.</param>
        /// <param name="numberOfSymbols">The number of symbols to include in the generated password.</param>
        /// <param name="noUpperCase">Flag indicating whether no upper case symbols should be included in the generated password.</param>
        /// <param name="allowRepeatedCharacters">Flag indicating whether repeated characters should be allowed in the generated password.</param>
        /// <returns>A random password string with the requirements specified by the parameters</returns>
        /// <exception cref="ArgumentException">Thrown when the password requirements cannot be met.</exception>
        public string Generate(int length, int numberOfDigits, int numberOfSymbols, bool noUpperCase = false, bool allowRepeatedCharacters = false)
        {
            var letters = LowerCaseLetters;
            if (!noUpperCase)
            {
                letters += UpperCaseLetters;
            }

            var charactersInPassword = length - numberOfDigits - numberOfSymbols;

            if (charactersInPassword < 0)
            {
                throw new ArgumentException("Number of digits and symbols must be less than length.", nameof(length));
            }

            if (!allowRepeatedCharacters && charactersInPassword > letters.Length)
            {
                throw new ArgumentException("Number of characters requested exceeds available letters and repeats are not allowed", nameof(length));
            }

            if (!allowRepeatedCharacters && numberOfDigits > 0 && numberOfDigits > Digits.Length)
            {
                throw new ArgumentException("Number of digits requested exceeds available digits and repeats are not allowed", nameof(numberOfDigits));
            }

            if (!allowRepeatedCharacters && numberOfSymbols > 0 && numberOfSymbols > Symbols.Length)
            {
                throw new ArgumentException("Number of symbols requested exceeds available symbols and repeats are not allowed", nameof(numberOfSymbols));
            }

            string result = string.Empty;

            for (int i = 0; i < charactersInPassword; i++)
            {
                var character = GetRandomElement(letters);

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
                var digit = GetRandomElement(Digits);

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
                var symbol = GetRandomElement(Symbols);

                if (!allowRepeatedCharacters && result.Contains(Symbols, StringComparison.InvariantCulture))
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
        /// Generates a random password string with the requirements specified, using the approach described by AgileBytes 1Password.
        /// These types of passwords are typically used for web site passwords.
        /// </summary>
        /// <remarks>
        /// The AgileBytes 1Password algorithm is described at https://1password.community/discussion/23842/how-random-are-the-generated-passwords.
        /// </remarks>
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
        /// <exception cref="ArgumentException">Thrown when the password requirements cannot be met.</exception>
        public string Generate(
            int length,
            int numberOfDigits,
            int numberOfSymbols,
            bool noUpperCase,
            bool allowRepeatedCharacters,
            string lowerCaseLetters,
            string upperCaseLetters,
            string digits,
            string symbols)
        {
            LowerCaseLetters = lowerCaseLetters;
            UpperCaseLetters = upperCaseLetters;
            Digits = digits;
            Symbols = symbols;

            return Generate(length, numberOfDigits, numberOfSymbols, noUpperCase, allowRepeatedCharacters);
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
            return (input.Insert(position, characterToInsert.ToString()));
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
}
