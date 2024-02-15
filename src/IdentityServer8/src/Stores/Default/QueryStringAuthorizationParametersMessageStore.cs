/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

namespace IdentityServer8.Stores;

// internal just for testing
internal class QueryStringAuthorizationParametersMessageStore : IAuthorizationParametersMessageStore
{
    public Task<string> WriteAsync(Message<IDictionary<string, string[]>> message)
    {
        var queryString = message.Data.FromFullDictionary().ToQueryString();
        return Task.FromResult(queryString);
    }

    public Task<Message<IDictionary<string, string[]>>> ReadAsync(string id)
    {
        var values = id.ReadQueryStringAsNameValueCollection();
        var msg = new Message<IDictionary<string, string[]>>(values.ToFullDictionary());
        return Task.FromResult(msg);
    }

    public Task DeleteAsync(string id)
    {
        return Task.CompletedTask;
    }
}
