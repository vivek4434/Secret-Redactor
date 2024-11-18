namespace Secret.Redactor.TwoPointer.Interfaces
{
    using System.Collections.Generic;
    using System.Net.Sockets;

    public interface IHasher
    {
        /// <summary>
        /// Updates the hash values with the new character.
        /// </summary>
        /// <param name="c">The new character to add to the hash.</param>
        /// <param name="hash31">The current hash value using base 31.</param>
        /// <param name="hash257">The current hash value using base 257.</param>
        /// <param name="power31">The current power value using base 31.</param></param>
        /// <param name="power257">The current power value using base 257.</param>
        /// <returns>A tuple containing the updated hash values.</returns>
        (long, long, long, long) UpdateHashes(char c, long hash31, long hash257, long power31, long power257);

        /// <summary>
        /// Removes the contribution of a character from the hash value.
        /// </summary>
        /// <param name="character">The character to be removed from the hash.</param>
        /// <param name="hash31">The current hash31 value.</param>
        /// <param name="hash257">The current hash257 value.</param>
        /// <param name="power31">The current power31 value.</param>
        /// <param name="power257">The current power257 value.</param>
        /// <returns>The updated hash values after removing the character's contribution.</returns>
        (long, long, long, long) RemoveHash(char character, long hash31, long hash257, long power31, long power257);

        /// <summary>
        /// Checks if the given hash values match any of the precomputed hashes.
        /// </summary>
        /// <param name="hash31">The hash value using base 31.</param>
        /// <param name="hash257">The hash value using base 257.</param>
        /// <returns>True if the hash values match; otherwise, false.</returns>
        bool Match(long hash31, long hash257);

        /// <summary>
        /// Generates the hash values for the given secret substrings.
        /// </summary>
        /// <param name="secrets">The collection of secret substrings.</param>
        void GenerateHashes(IEnumerable<string> secrets);

        /// <summary>
        /// Updates the set of secret characters.
        /// </summary>
        /// <param name="newSecretCharacters">The new secret characters to add.</param>
        void UpdateSecretCharacters(IEnumerable<char> newSecretCharacters);

        /// <summary>
        /// Checks if a character is part of the secret character set.
        /// </summary>
        /// <param name="c">The character to check.</param>
        /// <returns>True if the character is a secret character; otherwise, false.</returns>
        bool IsSecretCharacter(char c);
    }
}
