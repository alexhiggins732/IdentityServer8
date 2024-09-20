/*
 Copyright (c) 2024, Martijn van Put - https://github.com/mvput/ 

 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

namespace IdentityServer.Models;

/// <summary>
/// Base class for data that needs to be written out as cookies.
/// </summary>
public class Message<TModel>
{
    /// <summary>
    /// Should only be used from unit tests
    /// </summary>
    /// <param name="data"></param>
    internal Message(TModel data) : this(data, DateTime.UtcNow)
    {
    }

    /// <summary>
    /// For JSON serializer. 
    /// System.Text.Json.JsonSerializer requires public, parameterless constructor
    /// </summary>
    public Message()
    {
    }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="Message{TModel}"/> class.
    /// </summary>
    /// <param name="data">The data.</param>
    /// <param name="now">The current UTC date/time.</param>
    public Message(TModel data, DateTime now)
    {
        Created = now.Ticks;
        Data = data;
    }

    /// <summary>
    /// Gets or sets the UTC ticks the <see cref="Message{TModel}" /> was created.
    /// </summary>
    /// <value>
    /// The created UTC ticks.
    /// </value>
    public long Created { get; set; }

    /// <summary>
    /// Gets or sets the data.
    /// </summary>
    /// <value>
    /// The data.
    /// </value>
    public TModel Data { get; set; }
}
