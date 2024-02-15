/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

namespace IdentityServer8.Models;

/// <summary>
/// Extension methods for hashing strings
/// </summary>
public static class HashExtensions
{
    /// <summary>
    /// Creates a SHA256 hash of the specified input.
    /// </summary>
    /// <param name="input">The input.</param>
    /// <returns>A hash</returns>
    public static string Sha256(this string input)
    {
        if (input.IsMissing()) return string.Empty;

        using (var sha = SHA256.Create())
        {
            var bytes = Encoding.UTF8.GetBytes(input);
            var hash = sha.ComputeHash(bytes);

            return Convert.ToBase64String(hash);
        }
    }

    /// <summary>
    /// Creates a SHA256 hash of the specified input.
    /// </summary>
    /// <param name="input">The input.</param>
    /// <returns>A hash.</returns>
    public static byte[] Sha256(this byte[] input)
    {
        if (input == null)
        {
            return null;
        }

        using (var sha = SHA256.Create())
        {
            return sha.ComputeHash(input);
        }
    }

    /// <summary>
    /// Creates a SHA512 hash of the specified input.
    /// </summary>
    /// <param name="input">The input.</param>
    /// <returns>A hash</returns>
    public static string Sha512(this string input)
    {
        if (input.IsMissing()) return string.Empty;

        using (var sha = SHA512.Create())
        {
            var bytes = Encoding.UTF8.GetBytes(input);
            var hash = sha.ComputeHash(bytes);

            return Convert.ToBase64String(hash);
        }
    }
}
