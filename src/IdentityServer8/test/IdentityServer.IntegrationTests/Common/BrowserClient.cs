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

namespace IdentityServer.IntegrationTests.Common
{
    public class BrowserClient : HttpClient
    {
        public BrowserClient(BrowserHandler browserHandler)
            : base(browserHandler)
        {
            BrowserHandler = browserHandler;
        }

        public BrowserHandler BrowserHandler { get; private set; }

        public bool AllowCookies
        {
            get { return BrowserHandler.AllowCookies; }
            set { BrowserHandler.AllowCookies = value; }
        }
        public bool AllowAutoRedirect
        {
            get { return BrowserHandler.AllowAutoRedirect; }
            set { BrowserHandler.AllowAutoRedirect = value; }
        }
        public int ErrorRedirectLimit
        {
            get { return BrowserHandler.ErrorRedirectLimit; }
            set { BrowserHandler.ErrorRedirectLimit = value; }
        }
        public int StopRedirectingAfter
        {
            get { return BrowserHandler.StopRedirectingAfter; }
            set { BrowserHandler.StopRedirectingAfter = value; }
        }

        internal void RemoveCookie(string uri, string name)
        {
            BrowserHandler.RemoveCookie(uri, name);
        }

        internal System.Net.Cookie GetCookie(string uri, string name)
        {
            return BrowserHandler.GetCookie(uri, name);
        }
    }
}
