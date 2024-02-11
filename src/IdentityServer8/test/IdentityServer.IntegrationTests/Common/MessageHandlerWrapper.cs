/*
 Copyright (c) 2024 HigginsSoft
 Written by Alexander Higgins https://github.com/alexhiggins732/ 
 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code for this software can be found at https://github.com/alexhiggins732/IdentityServer8

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.

*/

using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityServer.IntegrationTests.Common
{
    public class MessageHandlerWrapper : DelegatingHandler
    {
        public HttpResponseMessage Response { get; set; }

        public MessageHandlerWrapper(HttpMessageHandler handler)
            : base(handler)
        {
        }

        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Response = await base.SendAsync(request, cancellationToken);
            return Response;
        }
    }
}
