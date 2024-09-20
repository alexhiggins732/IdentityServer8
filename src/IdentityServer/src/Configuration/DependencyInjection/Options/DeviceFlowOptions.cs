/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

namespace IdentityServer8.Configuration;

/// <summary>
/// Configures device flow
/// </summary>
public class DeviceFlowOptions
{
    /// <summary>
    /// Gets or sets the default type of the user code.
    /// </summary>
    /// <value>
    /// The default type of the user code.
    /// </value>
    public string DefaultUserCodeType { get; set; } = IdentityServerConstants.UserCodeTypes.Numeric;

    /// <summary>
    /// Gets or sets the polling interval in seconds.
    /// </summary>
    /// <value>
    /// The interval in seconds.
    /// </value>
    public int Interval { get; set; } = 5;
}
