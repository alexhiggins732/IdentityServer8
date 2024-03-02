/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

using Microsoft.Extensions.DependencyInjection;
using Xunit;


namespace IdentityServer8.Security.Tests;

public class DependencyInjection
{
    static DependencyInjection()
    {
        var provider = new ServiceCollection()
            .AddLogging()
            .AddAllowAnyRedirectService()
            .AddSingleton<IRedirectService, RedirectService>()
            .AddSanitizers()
            .BuildServiceProvider();
        ServiceProvider = provider;

    }

    public static ServiceProvider ServiceProvider { get; }
}



public class AllowAnyTests
{
    private readonly RedirectService _redirectService;

    public AllowAnyTests()
    {
        var redirectService = DependencyInjection.ServiceProvider.GetRequiredService<AllowAnyRedirectService>();
        _redirectService = redirectService;
    }


    [Theory]
    [InlineData("http://example.com", true)]
    [InlineData("http://a.example.com", true)]
    [InlineData("http://a.b.example.com", true)]
    [InlineData("https://example.com", true)]
    [InlineData("https://a.example.com", true)]
    [InlineData("https://a.b.example.com", true)]
    [InlineData("http://localhost", true)]
    [InlineData("https://localhost", true)]
    public void AllowAnyShouldReturnTrueUnlessInvalidUri(string uriString, bool expectedResult)
    {
        var result = _redirectService.IsRedirectAllowed(uriString);

        Assert.Equal(expectedResult, result);
    }


}
public class RedirectServiceTests
{
    private readonly RedirectService _redirectService;

    public RedirectServiceTests()
    {
        var redirectService = DependencyInjection.ServiceProvider.GetService<IRedirectService>();
        _redirectService = redirectService as RedirectService;
    }

    [Theory]
    [InlineData("http://example.com", "example.com", true)]
    [InlineData("http://example.com", "*", true)]
    [InlineData("http://example.com", "another.com", false)]
    [InlineData("example.com", "example.com", true)]
    [InlineData("example.com", "*", true)]
    [InlineData("example.com", "another.com", false)]
    [InlineData("*.example.com", "example.com", false)]
    [InlineData("*.example.com", "*", true)]
    [InlineData("*.example.com", "another.com", false)]
    public void IsHostMatch_ShouldCorrectlyMatchHost(string uriString, string allowedHostName, bool expectedResult)
    {

        var allowedHost = allowedHostName == "*" ? Host.Any : Host.Create(allowedHostName);

        // Assuming IsHostMatch is made internal for testing and accessible here
        var result = _redirectService.IsHostMatch(uriString, allowedHost);

        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [InlineData("", "*", true)]
    [InlineData("http", "", true)]
    [InlineData("https", "", true)]
    [InlineData("http", "*", true)]
    [InlineData("https", "*", true)]
    [InlineData("", "http", false)]
    [InlineData("http", "http", true)]
    [InlineData("https", "http", false)]

    [InlineData("", "https", false)]
    [InlineData("http", "https", false)]
    [InlineData("https", "https", true)]
    public void IsAllowedScheme_ShouldCorrectlyMatchPath(string actual, string allowed, bool expected)
    {
        var scheme = Scheme.Parse(allowed);
        var result = _redirectService.IsSchemeMatch(actual, scheme);

        Assert.Equal(expected, result);
    }

    [Fact()]
    public void AddARedirectRuleTest()
    {

    }

    [Fact()]
    public void IsRedirectAllowedTest()
    {

    }
}
