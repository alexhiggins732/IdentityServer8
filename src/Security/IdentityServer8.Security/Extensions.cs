/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

namespace Microsoft.Extensions.DependencyInjection;

public class Ioc
{
    static Ioc()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddSanitizers();
        services.AddAllowAnyRedirectService();
        services.AddSingleton<IRedirectService, AllowAnyRedirectService>();
        ServiceProvider = services.BuildServiceProvider();
        var sanitizer = ServiceProvider.GetRequiredService<ISanitizer>();
        Sanitizer = sanitizer;
        var redirectService = ServiceProvider.GetRequiredService<IRedirectService>();
        RedirectService = redirectService;
    }

    public static ServiceProvider ServiceProvider { get; }
    public static ISanitizer Sanitizer { get; }
    public static IRedirectService RedirectService { get; }

}
