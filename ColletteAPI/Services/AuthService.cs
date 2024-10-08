/*
 * File: AuthService.cs
 * Description: This file contains the implementation of the AuthService class,
 * which provides methods for password hashing and verification for secure user authentication.
 */

using System.Security.Cryptography;
using System.Text;

namespace ColletteAPI.Services
{
    /*
     * Class: AuthService
     * This service provides methods for hashing passwords and verifying passwords against stored hashes.
     * It uses the SHA-256 algorithm for secure password encryption.
     */
    public class AuthService
    {
        
        /*
         * Method: HashPassword
         * Hashes the provided password using the SHA-256 algorithm and returns the hashed value as a Base64 string.
         *
         * Parameters:
         *  - password: The plain text password to be hashed.
         * 
         * Returns:
         *  - The hashed password as a Base64-encoded string.
         */
        public string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes); // Return the hashed value as a Base64 string
            }
        }

        /*
         * Method: VerifyPassword
         * Verifies whether the provided plain text password matches the stored hashed password.
         * 
         * Parameters:
         *  - password: The plain text password to be verified.
         *  - storedHash: The stored hashed password to compare against.
         * 
         * Returns:
         *  - True if the hashed password matches the stored hash, false otherwise.
         */
        public bool VerifyPassword(string password, string storedHash)
        {
            var hashedPassword = HashPassword(password); // Hash the provided password
            return hashedPassword == storedHash; // Compare the hashed password with the stored hash
        }
    }
}
