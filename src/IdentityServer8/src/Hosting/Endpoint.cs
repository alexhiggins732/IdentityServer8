/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

#pragma warning disable 1591


namespace IdentityServer8.Hosting;

public class Endpoint
{
    public Endpoint(string name, string path, Type handlerType)
    {
        Name = name;
        Path = path;
        Handler = handlerType;
    }

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    /// <value>
    /// The name.
    /// </value>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the path.
    /// </summary>
    /// <value>
    /// The path.
    /// </value>
    public PathString Path { get; set; }

    /// <summary>
    /// Gets or sets the handler.
    /// </summary>
    /// <value>
    /// The handler.
    /// </value>
    public Type Handler { get; set; }
}
