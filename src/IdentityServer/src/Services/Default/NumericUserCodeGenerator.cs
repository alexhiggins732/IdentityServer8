/*
 Copyright (c) 2024, Martijn van Put - https://github.com/mvput/ 

 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

namespace IdentityServer.Services;

/// <summary>
/// User code generator using 9 digit number
/// </summary>
/// <seealso cref="IdentityServer.Services.IUserCodeGenerator" />
public class NumericUserCodeGenerator : IUserCodeGenerator
{
    /// <summary>
    /// Gets the type of the user code.
    /// </summary>
    /// <value>
    /// The type of the user code.
    /// </value>
    public string UserCodeType => IdentityServerConstants.UserCodeTypes.Numeric;

    /// <summary>
    /// Gets the retry limit.
    /// </summary>
    /// <value>
    /// The retry limit for getting a unique value.
    /// </value>
    public int RetryLimit => 5;

    /// <summary>
    /// Generates the user code.
    /// </summary>
    /// <returns></returns>
    public Task<string> GenerateAsync()
    {
        var next = Next(100000000, 999999999);
        return Task.FromResult(next.ToString());
    }

    private int Next(int minValue, int maxValue)
    {
        if (minValue > maxValue) throw new ArgumentOutOfRangeException(nameof(minValue));
        if (minValue == maxValue) return minValue;
        long diff = maxValue - minValue;

        var uint32Buffer = new byte[8];

        using (var rng = new RNGCryptoServiceProvider())
        {
            while (true)
            {
                rng.GetBytes(uint32Buffer);
                var rand = BitConverter.ToUInt32(uint32Buffer, 0);

                const long max = 1 + (long)uint.MaxValue;
                var remainder = max % diff;
                if (rand < max - remainder)
                {
                    return (int)(minValue + rand % diff);
                }
            }
        }
    }
}
