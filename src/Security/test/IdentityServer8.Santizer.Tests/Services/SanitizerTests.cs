/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using System.Text.Json;
using Xunit;

namespace IdentityServer8.Security.Tests;


public class SanitizerTests
{
    public SanitizerTests()
    {

    }


    [Fact]
    public void SanitizerFactory()
    {
        var factory = Ioc.ServiceProvider.GetRequiredService<ISanitizerFactory>();
        Assert.NotNull(factory);
    }

    [Fact]
    public void SanitizerService()
    {
        var service = Ioc.ServiceProvider.GetRequiredService<ISanitizerService>();
        Assert.NotNull(service);
    }
    [Fact]
    public void Sanitizer()
    {
        var sanitizer = Ioc.ServiceProvider.GetRequiredService<ISanitizer>();
        Assert.NotNull(sanitizer);
    }


    [Fact]
    public void HtmlSanitizer()
    {
        var sanitizer = Ioc.ServiceProvider.GetRequiredService<IHtmlSanitizer>();
        Assert.NotNull(sanitizer);

        var input = "<div><script>alert('xss')</script></div>";
        var output = sanitizer.Sanitize(input);
        var expected = "&lt;div&gt;&lt;script&gt;alert('xss')&lt;/script&gt;&lt;/div&gt;";

        Validate(expected, output);
    }
    [Fact]
    public void XmlSanitizer()
    {
        var sanitizer = Ioc.ServiceProvider.GetRequiredService<IXmlSanitizer>();
        Assert.NotNull(sanitizer);

        var input = "<div><script>alert('xss')</script></div>";
        var output = sanitizer.Sanitize(input);
        var expected = "&lt;div&gt;&lt;script&gt;alert('xss')&lt;/script&gt;&lt;/div&gt;";

        Validate(expected, output);
    }
    [Fact]
    public void JsonSanitizer()
    {
        var sanitizer = Ioc.ServiceProvider.GetRequiredService<IJsonSanitizer>();
        Assert.NotNull(sanitizer);

        var input = JsonSerializer.Serialize(new { test = "test", value = "<div><script>alert('xss')</script></div>" });
        var output = sanitizer.Sanitize(input);
        var expected = @"{\""test\"":\""test\"",\""value\"":\""\\u003Cdiv\\u003E\\u003Cscript\\u003Ealert(\\u0027xss\\u0027)\\u003C/script\\u003E\\u003C/div\\u003E\""}";


        Validate(expected, output);
    }
    [Fact]
    public void UrlSanitizer()
    {
        var sanitizer = Ioc.ServiceProvider.GetRequiredService<IUrlSanitizer>();
        Assert.NotNull(sanitizer);

        var input = "http://test.com?test=<div><script>alert('xss')</script></div>";
        var output = sanitizer.Sanitize(input);
        var expected = "http://test.com?test=%3Cdiv%3E%3Cscript%3Ealert('xss')%3C/script%3E%3C/div%3E";

        Validate(expected, output);
    }
    [Fact]
    public void CssSanitizer()
    {
        var sanitizer = Ioc.ServiceProvider.GetRequiredService<ICssSanitizer>();
        Assert.NotNull(sanitizer);

        var input = "div { background-url('<script>alert('xss')</script>') }";
        var output = sanitizer.Sanitize(input);
        var expected = "div { background-url('&lt;script&gt;alert('xss')&lt;/script&gt;') }";
        Validate(expected, output);
    }
    [Fact]
    public void ScriptSanitizer()
    {
        var sanitizer = Ioc.ServiceProvider.GetRequiredService<IJsonSanitizer>();
        Assert.NotNull(sanitizer);

        var input = JsonSerializer.Serialize(new { test = "test", value = "<div><script>alert('xss')</script></div>" });
        var output = sanitizer.Sanitize(input);
        var expected = @"{\""test\"":\""test\"",\""value\"":\""\\u003Cdiv\\u003E\\u003Cscript\\u003Ealert(\\u0027xss\\u0027)\\u003C/script\\u003E\\u003C/div\\u003E\""}";


        Validate(expected, output);
    }
    [Fact]
    public void StyleSanitizer()
    {
        var sanitizer = Ioc.ServiceProvider.GetRequiredService<IStyleSanitizer>();
        Assert.NotNull(sanitizer);
        var input = "div { background-url('<script>alert('xss')</script>') }";
        var output = sanitizer.Sanitize(input);
        var expected = "div { background-url('&lt;script&gt;alert('xss')&lt;/script&gt;') }";

        Validate(expected, output);
    }
    [Fact]
    public void SqlSanitizer()
    {
        var sanitizer = Ioc.ServiceProvider.GetRequiredService<ISqlSanitizer>();
        Assert.NotNull(sanitizer);

        var input = "update table set value='<script>alert('xss')</script>' where 1=1;";
        var output = sanitizer.Sanitize(input);
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
        var output = sanitizer.Sanitize(input);


        var expected = "log poisoning test &lt;script&gt;alert('xss')&lt;/script&gt;";
        Validate(expected, output);


        output = sanitizer.Sanitize("mypassword", SanitizerMode.Mask);
        expected = "******word";
        Validate(expected, output);


        output = sanitizer.Sanitize("mypassword", SanitizerMode.Full);
        expected = "********";
        Validate(expected, output);

    }

    void Validate(string expected, string output)
    {
        Console.WriteLine("Expected: " + expected);
        Console.WriteLine("Output: " + output);
        Debug.WriteLine("Expected: " + expected);
        Debug.WriteLine("Output: " + output);
        output.Should().Be(expected);
    }

    private void Log(string expected, string output)
    {
        Console.WriteLine("Expected: " + expected);
        Console.WriteLine("Output: " + output);
        Debug.WriteLine("Expected: " + expected);
        Debug.WriteLine("Output: " + output);
    }
}
