/*
 Copyright (c) 2024, Martijn van Put - https://github.com/mvput/ 

 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

namespace IdentityServer.Validation;

/// <summary>
/// Context for client configuration validation.
/// </summary>
public class ClientConfigurationValidationContext
{
    /// <summary>
    /// Gets or sets the client.
    /// </summary>
    /// <value>
    /// The client.
    /// </value>
    public Client Client { get; }

    /// <summary>
    /// Returns true if client configuration is valid.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is valid; otherwise, <c>false</c>.
    /// </value>
    public bool IsValid { get; set; } = true;

    /// <summary>
    /// Gets or sets the error message.
    /// </summary>
    /// <value>
    /// The error message.
    /// </value>
    public string ErrorMessage { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ClientConfigurationValidationContext"/> class.
    /// </summary>
    /// <param name="client">The client.</param>
    public ClientConfigurationValidationContext(Client client)
    {
        Client = client;
    }

    /// <summary>
    /// Sets a validation error.
    /// </summary>
    /// <param name="message">The message.</param>
    public void SetError(string message)
    {
        IsValid = false;
        ErrorMessage = message;
    }
}
