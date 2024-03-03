/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

using FluentAssertions;
using FluentAssertions.Equivalency;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using System.Reflection;
using System.Text.Json;
using Xunit;

namespace IdentityServer8.Security.Tests;


public class SanitizerExtensionTests
{
    public SanitizerExtensionTests()
    {

    }



    [Fact]
    public void HtmlSanitizer()
    {
        var sanitizer = Ioc.ServiceProvider.GetRequiredService<IHtmlSanitizer>();
        Assert.NotNull(sanitizer);

        var input = "<div><script>alert('xss')</script></div>";
        var output = input.SanitizeForHtml();
        var expected = "&lt;div&gt;&lt;script&gt;alert('xss')&lt;/script&gt;&lt;/div&gt;";

        Validate(expected, output);
    }
    [Fact]
    public void XmlSanitizer()
    {
        var sanitizer = Ioc.ServiceProvider.GetRequiredService<IXmlSanitizer>();
        Assert.NotNull(sanitizer);

        var input = "<div><script>alert('xss')</script></div>";
        var output = input.SanitizeForXml();
        var expected = "&lt;div&gt;&lt;script&gt;alert('xss')&lt;/script&gt;&lt;/div&gt;";

        Validate(expected, output);
    }
    [Fact]
    public void JsonSanitizer()
    {
        var sanitizer = Ioc.ServiceProvider.GetRequiredService<IJsonSanitizer>();
        Assert.NotNull(sanitizer);

        var input = JsonSerializer.Serialize(new { test = "test", value = "<div><script>alert('xss')</script></div>" });
        var output = input.SanitizeForJson();
        var expected = @"{\""test\"":\""test\"",\""value\"":\""\\u003Cdiv\\u003E\\u003Cscript\\u003Ealert(\\u0027xss\\u0027)\\u003C/script\\u003E\\u003C/div\\u003E\""}";


        Validate(expected, output);
    }
    [Fact]
    public void UrlSanitizer()
    {
        var sanitizer = Ioc.ServiceProvider.GetRequiredService<IUrlSanitizer>();
        Assert.NotNull(sanitizer);

        var input = "http://test.com?test=<div><script>alert('xss')</script></div>";
        var output = input.SanitizeForUrl();
        var expected = "http://test.com?test=%3Cdiv%3E%3Cscript%3Ealert('xss')%3C/script%3E%3C/div%3E";

        Validate(expected, output);
    }
    [Fact]
    public void CssSanitizer()
    {
        var sanitizer = Ioc.ServiceProvider.GetRequiredService<ICssSanitizer>();
        Assert.NotNull(sanitizer);

        var input = "div { background-url('<script>alert('xss')</script>') }";
        var output = input.SanitizeForCss();
        var expected = "div { background-url('&lt;script&gt;alert('xss')&lt;/script&gt;') }";
        Validate(expected, output);
    }
    [Fact]
    public void ScriptSanitizer()
    {
        var sanitizer = Ioc.ServiceProvider.GetRequiredService<IJsonSanitizer>();
        Assert.NotNull(sanitizer);

        var input = "<script>alert('xss')</script>";
        sanitizer.Sanitize(input);
        var output = input.SanitizeForScript();
        var expected = @"&lt;script&gt;alert('xss')&lt;/script&gt;";


        Validate(expected, output);
    }
    [Fact]
    public void StyleSanitizer()
    {
        var sanitizer = Ioc.ServiceProvider.GetRequiredService<IStyleSanitizer>();
        Assert.NotNull(sanitizer);
        var input = "div { background-url('<script>alert('xss')</script>') }";
        var output = input.SanitizeForStyle();
        var expected = "div { background-url('&lt;script&gt;alert('xss')&lt;/script&gt;') }";

        Validate(expected, output);
    }
    [Fact]
    public void SqlSanitizer()
    {
        var sanitizer = Ioc.ServiceProvider.GetRequiredService<ISqlSanitizer>();
        Assert.NotNull(sanitizer);

        var input = "update table set value='<script>alert('xss')</script>' where 1=1;";
        var output = input.SanitizeForSql();
        var expected = "update table set value='&lt;script&gt;alert('xss')&lt;/script&gt;' where 1=1;";

        Validate(expected, output);
    }
    [Fact]
    public void LogSanitizer()
    {
        var sanitizer = Ioc.ServiceProvider.GetRequiredService<ILogSanitizer>();
        Assert.NotNull(sanitizer);

        var input = @"log
poisoning
test

<script>alert('xss')</script>
";
        var output = input.SanitizeForLog();


        var expected = "log poisoning test &lt;script&gt;alert('xss')&lt;/script&gt;";
        Validate(expected, output);


        output = input.SanitizeForLog(SanitizerMode.Mask);
        expected = "******isoning test &lt;script&gt;alert('xss')&lt;/script&gt;";
        Validate(expected, output);


        output = input.SanitizeForLog(SanitizerMode.Full);
        expected = "********";
        Validate(expected, output);

    }


    [Fact]
    public void SanitizersConvertNullToEmpty()
    {
        string? input = null;
        input.SanitizeForHtml().Should().Be("");
        input.SanitizeForXml().Should().Be("");
        input.SanitizeForJson().Should().Be("");
        input.SanitizeForUrl().Should().Be("");
        input.SanitizeForCss().Should().Be("");
        input.SanitizeForScript().Should().Be("");
        input.SanitizeForStyle().Should().Be("");
        input.SanitizeForSql().Should().Be("");
        input.SanitizeForRedirect().Should().Be("");
    }

    [Fact]
    public void LogSaniziterAllowsNull()
    {
        string? input = null;
        input.SanitizeForLog().Should().BeNull();
    }

    [Fact]
    public void SanitizersTrimInput()
    {
        string input = "test ";
        var expected = input.Trim();
        input.SanitizeForHtml().Should().Be(expected);
        input.SanitizeForXml().Should().Be(expected);
        input.SanitizeForJson().Should().Be(expected);
        input.SanitizeForCss().Should().Be(expected);
        input.SanitizeForScript().Should().Be(expected);
        input.SanitizeForStyle().Should().Be(expected);
        input.SanitizeForSql().Should().Be(expected);
        input.SanitizeForLog().Should().Be(expected);
    }

    [Fact]
    public void UrlSanitizerTrimsInput()
    {
        string input = "/test ";
        var expected = "/test";
        input.SanitizeForUrl().Should().Be(expected);
        input.SanitizeForRedirect().Should().Be(expected);
    }



    [Fact]
    public void UriToString_DiffersFromRawUrl_WithEscapedCharacters()
    {
        // Arrange
        var rawUrl = "http://example.com/path with spaces";

        // Act
        if (Uri.TryCreate(rawUrl, UriKind.RelativeOrAbsolute, out var uri))
        {
            var result = uri.AbsoluteUri.ToString();

            // Assert
            Assert.NotEqual(rawUrl, result); // Uri.ToString() will escape the spaces, so it should not match the raw URL.
            Assert.Equal("http://example.com/path%20with%20spaces", result); // This is the expected normalization.
        }
    }

    [Fact]
    public void UriToString_DiffersFromRawUrl_WithDefaultPort()
    {
        // Arrange
        var rawUrl = "http://example.com:80/path";

        // Act
        if (Uri.TryCreate(rawUrl, UriKind.RelativeOrAbsolute, out var uri))
        {
            var result = uri.ToString();

            // Assert
            Assert.NotEqual(rawUrl, result); // Uri.ToString() will remove the default port, so it should not match the raw URL.
            Assert.Equal("http://example.com/path", result); // This is the expected normalization.
        }

    }

    [Fact(Skip = "encoding urls breaks authorize and connect endpoints in certain scenarios")]
    public void SanitizeForRedirect()
    {
        string? s = null;
        s.SanitizeForRedirect().Should().Be("");
        "http://example.com:80/path".SanitizeForRedirect().Should().Be("http://example.com/path");
        "http://example.com/path with spaces".SanitizeForRedirect().Should().Be("http://example.com/path%20with%20spaces");
    }

    void Validate(string expected, string output)
    {
        //Console.WriteLine("Expected: " + expected);
        //Console.WriteLine("Output: " + output);
        //Debug.WriteLine("Expected: " + expected);
        //Debug.WriteLine("Output: " + output);
        output.Should().Be(expected);
    }

}
