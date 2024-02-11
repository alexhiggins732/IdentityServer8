/*
 Copyright (c) 2024 HigginsSoft
 Written by Alexander Higgins https://github.com/alexhiggins732/ 
 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code for this software can be found at https://github.com/alexhiggins732/IdentityServer8

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.

*/

using IdentityServer8.Models;

namespace IdentityServer8.Validation
{
    /// <summary>
    /// Validation result for client validation
    /// </summary>
    public class ClientSecretValidationResult : ValidationResult
    {
        /// <summary>
        /// Gets or sets the client.
        /// </summary>
        /// <value>
        /// The client.
        /// </value>
        public Client Client { get; set; }

        /// <summary>
        /// Gets or sets the secret used to authenticate the client.
        /// </summary>
        /// <value>
        /// The secret.
        /// </value>
        public ParsedSecret Secret { get; set; }

        /// <summary>
        /// Gets or sets the value of the confirmation method (will become the cnf claim). Must be a JSON object.
        /// </summary>
        /// <value>
        /// The confirmation.
        /// </value>
        public string Confirmation { get; set; }
    }
}