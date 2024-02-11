/*
 Copyright (c) 2024 HigginsSoft
 Written by Alexander Higgins https://github.com/alexhiggins732/ 
 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code for this software can be found at https://github.com/alexhiggins732/IdentityServer8

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.

*/

using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using IdentityModel;

namespace IdentityServer8.Extensions
{
    /// <summary>
    /// Extensions methods for X509Certificate2
    /// </summary>
    public static class X509CertificateExtensions
    {
        /// <summary>
        /// Create the value of a thumbprint-based cnf claim
        /// </summary>
        /// <param name="certificate"></param>
        /// <returns></returns>
        public static string CreateThumbprintCnf(this X509Certificate2 certificate)
        {
            var hash = certificate.GetCertHash(HashAlgorithmName.SHA256);
                            
            var values = new Dictionary<string, string>
            {
                { "x5t#S256", Base64Url.Encode(hash) }
            };

            return JsonSerializer.Serialize(values);
        }
    }
}