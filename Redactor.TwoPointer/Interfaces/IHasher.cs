namespace Secret.Redactor.TwoPointer.Interfaces
{
    using System.Collections.Generic;

    public interface IHasher
    {
        /// <summary>
        /// Updates the hash values with the new character.
        /// </summary>
        /// <param name="c">The new character to add to the hash.</param>
        /// <param name="hash31">The current hash value using base 31.</param>
        /// <param name="hash257">The current hash value using base 257.</param>
        /// <returns>A tuple containing the updated hash values.</returns>
        (long, long) UpdateHashes(char c, long hash31, long hash257);

        /// <summary>
        /// Removes the contribution of a character from the hash value.
        /// </summary>
        /// <param name="c">The character to be removed from the hash.</param>
        /// <param name="hash">The current hash value.</param>
        /// <param name="baseValue">The base value used in the hash calculation (e.g., 31 or 257).</param>
        /// <returns>The updated hash value after removing the character's contribution.</returns>
        long RemoveHash(char c, long hash, int baseValue);

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
