/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

namespace IdentityServer8.Stores;

internal class ConsentMessageStore : IConsentMessageStore
{
    protected readonly MessageCookie<ConsentResponse> Cookie;

    public ConsentMessageStore(MessageCookie<ConsentResponse> cookie)
    {
        Cookie = cookie;
    }

    public virtual Task DeleteAsync(string id)
    {
        Cookie.Clear(id);
        return Task.CompletedTask;
    }

    public virtual Task<Message<ConsentResponse>> ReadAsync(string id)
    {
        return Task.FromResult(Cookie.Read(id));
    }

    public virtual Task WriteAsync(string id, Message<ConsentResponse> message)
    {
        Cookie.Write(id, message);
        return Task.CompletedTask;
    }
}
