/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Net;
using Xunit;
using static System.Runtime.InteropServices.JavaScript.JSType;


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
    private readonly RuleMatcher _ruleMatcher;
    public AllowAnyTests()
    {
        var redirectService = DependencyInjection.ServiceProvider.GetRequiredService<AllowAnyRedirectService>();
        _redirectService = redirectService;
        _ruleMatcher = new();
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
    //[InlineData("http:htps//example.com", false)]
    public void AllowAnyShouldReturnTrueUnlessInvalidUri(string uriString, bool expectedResult)
    {
        var result = _redirectService.IsRedirectAllowed(uriString);

        Assert.Equal(expectedResult, result);
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
    public void AllowAnyUriShouldReturnTrueUnlessInvalidUri(string uriString, bool expectedResult)
    {
        var uri = new Uri(uriString);
        var result = uri.IsAllowedRedirect();
        Assert.Equal(expectedResult, result);
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
    public void RetdirectRuleMatcher_AllowAnyUriShouldReturnTrueUnlessInvalidUri(string uriString, bool expectedResult)
    {
        var result = _ruleMatcher.IsMatch(uriString, RedirectRule.AllowAny);
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void RedirectValidation_RegistersRedirectService()
    {
        var provider = RedirectValidition.ServiceProvider;
        var redirectService = provider.GetRequiredService<IRedirectService>();
        redirectService.Should().NotBeNull();
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
    public void RetdirectService_AllowAnyUriShouldReturnTrueUnlessInvalidUri(string uriString, bool expectedResult)
    {
        var logger = new Mock<Microsoft.Extensions.Logging.ILogger<RedirectService>>();
        var sanitizer = new Mock<ISanitizer>();
        var service = new RedirectService(logger.Object, sanitizer.Object);
        service.ClearRules();
        service.AddRule(RedirectRule.AllowAny);
        var result = service.IsRedirectAllowed(uriString);
        result.Should().Be(expectedResult);

    }

    [Fact]
    public void RedirectService_CanAddAndRemoveRules()
    {
        var logger = new Mock<Microsoft.Extensions.Logging.ILogger<RedirectService>>();
        var sanitizer = new Mock<ISanitizer>();
        var service = new RedirectService(logger.Object, sanitizer.Object);
        service.ClearRules();
        service.GetRedirectRules().Count.Should().Be(0);
        var any = RedirectRule.AllowAny;
        service.AddRule(any);
        service.GetRedirectRules().Count.Should().Be(1);
        service.RemoveRule(any);
        service.GetRedirectRules().Count.Should().Be(0);
        service.AddRules([any]);
        service.GetRedirectRules().Count.Should().Be(1);
        service.RemoveRules([any]);
        service.GetRedirectRules().Count.Should().Be(0);

    }


    [Fact]
    public void SetRedirectNotAllowed_SetsStatusCodeToForbidden()
    {
        // Arrange
        var httpResponseMock = new Mock<HttpResponse>();
        httpResponseMock.SetupProperty(response => response.StatusCode);

        // Act
        httpResponseMock.Object.SetRedirectNotAllowed();

        // Assert
        Assert.Equal((int) HttpStatusCode.Forbidden, httpResponseMock.Object.StatusCode);
    }

   
    [Fact(Skip = "Not to allow safe rule changes that don't break parallel integration tests")]
    public void SetRedirectNotAllowed_SetsStatusCodeToForbiddenIfNotAllowed()
    {


         // Arrange
         var httpResponseMock = new Mock<HttpResponse>();
        httpResponseMock.SetupProperty(response => response.StatusCode);

        // Act
        httpResponseMock.Object.SetRedirectNotAllowed();

        var redirectService = (RedirectService) Ioc.RedirectService;
        var rules = redirectService.GetRedirectRules();
        redirectService.ClearRules();
        redirectService.AddRule(new RedirectRule { AllowedHost=Host.Create("example.com")});

        httpResponseMock.Object.RedirectIfAllowed("http://notexample.com");
        redirectService.AddRules(rules);
        // Assert
        Assert.Equal((int) HttpStatusCode.Forbidden, httpResponseMock.Object.StatusCode);
    }


    [Fact]
    public void SetRedirectNotAllowed_SetsResponseStatusCodeToForbidden()
    {
        // Arrange
        var httpResponseMock = new Mock<HttpResponse>();
        httpResponseMock.SetupProperty(response => response.StatusCode);

        var httpContextMock = new Mock<HttpContext>();
        httpContextMock.Setup(ctx => ctx.Response).Returns(httpResponseMock.Object);

        // Act
        httpContextMock.Object.SetRedirectNotAllowed();

        // Assert
        httpResponseMock.VerifySet(response => response.StatusCode = (int) HttpStatusCode.Forbidden, Times.Once);
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
    [InlineData("subdomain.example.com", "*.example.com", true)]
    [InlineData("", "another.com", false)]
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
    public void EmptyHostDoesNotMatchExplicitHost()
    {

    }


}
