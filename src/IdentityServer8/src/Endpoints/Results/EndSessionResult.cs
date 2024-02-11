/*
 Copyright (c) 2024 HigginsSoft
 Written by Alexander Higgins https://github.com/alexhiggins732/ 
 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code for this software can be found at https://github.com/alexhiggins732/IdentityServer8

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.

*/

using System.Threading.Tasks;
using IdentityServer8.Validation;
using IdentityServer8.Hosting;
using Microsoft.AspNetCore.Http;
using IdentityServer8.Configuration;
using Microsoft.Extensions.DependencyInjection;
using IdentityServer8.Models;
using IdentityServer8.Stores;
using IdentityServer8.Extensions;
using System;
using Microsoft.AspNetCore.Authentication;

namespace IdentityServer8.Endpoints.Results
{
    /// <summary>
    /// Result for endsession
    /// </summary>
    /// <seealso cref="IdentityServer8.Hosting.IEndpointResult" />
    public class EndSessionResult : IEndpointResult
    {
        private readonly EndSessionValidationResult _result;

        /// <summary>
        /// Initializes a new instance of the <see cref="EndSessionResult"/> class.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <exception cref="System.ArgumentNullException">result</exception>
        public EndSessionResult(EndSessionValidationResult result)
        {
            _result = result ?? throw new ArgumentNullException(nameof(result));
        }

        internal EndSessionResult(
            EndSessionValidationResult result,
            IdentityServerOptions options,
            ISystemClock clock,
            IMessageStore<LogoutMessage> logoutMessageStore)
            : this(result)
        {
            _options = options;
            _clock = clock;
            _logoutMessageStore = logoutMessageStore;
        }

        private IdentityServerOptions _options;
        private ISystemClock _clock;
        private IMessageStore<LogoutMessage> _logoutMessageStore;

        private void Init(HttpContext context)
        {
            _options = _options ?? context.RequestServices.GetRequiredService<IdentityServerOptions>();
            _clock = _clock ?? context.RequestServices.GetRequiredService<ISystemClock>();
            _logoutMessageStore = _logoutMessageStore ?? context.RequestServices.GetRequiredService<IMessageStore<LogoutMessage>>();
        }

        /// <summary>
        /// Executes the result.
        /// </summary>
        /// <param name="context">The HTTP context.</param>
        /// <returns></returns>
        public async Task ExecuteAsync(HttpContext context)
        {
            Init(context);

            var validatedRequest = _result.IsError ? null : _result.ValidatedRequest;

            string id = null;

            if (validatedRequest != null)
            {
                var logoutMessage = new LogoutMessage(validatedRequest);
                if (logoutMessage.ContainsPayload)
                {
                    var msg = new Message<LogoutMessage>(logoutMessage, _clock.UtcNow.UtcDateTime);
                    id = await _logoutMessageStore.WriteAsync(msg);
                }
            }

            var redirect = _options.UserInteraction.LogoutUrl;

            if (redirect.IsLocalUrl())
            {
                redirect = context.GetIdentityServerRelativeUrl(redirect);
            }

            if (id != null)
            {
                redirect = redirect.AddQueryString(_options.UserInteraction.LogoutIdParameter, id);
            }

            context.Response.Redirect(redirect);
        }
    }
}
