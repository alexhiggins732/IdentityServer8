/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/


// run a static files web server
var app = WebApplication.Create(args);
app
    .UseDefaultFiles()
    .UseStaticFiles();

// uncomment to enable to test w/ CSP (Content-Security-Policy)
/*
app.Use(async (ctx, next) =>
{
    ctx.Response.OnStarting(() =>
    {
        if (ctx.Response.ContentType?.StartsWith("text/html") == true)
        {
            ctx.Response.Headers.Add("Content-Security-Policy", "default-src 'self'; connect-src http://localhost:5000 http://localhost:3721; frame-src 'self' http://localhost:5000");
        }
        return Task.CompletedTask;
    });

    await next();
});
*/

await app.RunAsync();