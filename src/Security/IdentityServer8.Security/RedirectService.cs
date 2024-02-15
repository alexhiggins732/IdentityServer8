/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

namespace IdentityServer8.Security;

public interface IRedirectService
{
    bool IsRedirectAllowed(string redirectUrl);
}


public class RuleMatcher
{
    public bool IsMatch(string url, RedirectRule rule)
    {
        throw new NotImplementedException();
    }
}
public class RedirectRule
{
    public static RedirectRule AllowAny
        => new RedirectRule();

    public RedirectRule()
    {
        AllowedFragment = Fragment.Any;
        AllowedPath = Path.Any;
        AllowedPort = Port.Any;
        AllowedQuery = Query.Any;
        AllowedScheme = Scheme.Any;
        AllowedHost = Host.Any;
    }
    public Scheme AllowedScheme { get; set; }
    public Host AllowedHost { get; set; }
    public Port AllowedPort { get; set; }
    public Path AllowedPath { get; set; }
    public Query AllowedQuery { get; set; }
    public Fragment AllowedFragment { get; set; }
}

public class Scheme
{
    public Scheme(string name) { Name = name; }
    public string Name { get; }

    public static readonly Scheme Http = new Scheme("http");
    public static readonly Scheme Https = new Scheme("https");
    public static readonly Scheme Any = new Scheme("*");

    public static Scheme Parse(string schemeName)
    {
        if (string.IsNullOrWhiteSpace(schemeName))
        {
            throw new ArgumentException("Scheme name cannot be null or empty");
        }
        else if (schemeName.Equals("http", StringComparison.OrdinalIgnoreCase))
        {
            return Http;
        }
        else if (schemeName.Equals("https", StringComparison.OrdinalIgnoreCase))
        {
            return Https;
        }
        else if (schemeName.Equals("*", StringComparison.OrdinalIgnoreCase))
        {
            return Any;
        }
        else
        {
            throw new ArgumentException("Invalid scheme name");
        }

    }

    public override string ToString()
    {
        return Name;
    }
}

public class Host
{
    public Host(string value)
    {
        Value = ((value ?? "").ToLower().Trim() ?? "");
        var domainParts = Value.Split('.');
        domainParts = domainParts.Where(x => !string.IsNullOrEmpty(x)).ToArray();
        HostParts = new List<string>(domainParts);
    }
    public List<string> HostParts;

    public string Value { get; }

    public static readonly Host Any = new Host("*");

    public static Host Create(string hostname) => new Host(hostname);

    public bool HasWildcard => Value.Contains("*.");
    public bool IsAny => Value == "*";
    public bool IsLocalhost => Value.Equals("localhost", StringComparison.OrdinalIgnoreCase) || Value.Equals("127.0.0.1");
    public bool IsIpAddress => Uri.CheckHostName(Value) == UriHostNameType.IPv4 || Uri.CheckHostName(Value) == UriHostNameType.IPv6;
    public bool IsLocalNetwork => IsLocalhost || IsIpAddress;
    public bool IsFullQualifiedDomainName => !IsLocalNetwork && !HasWildcard && !IsAny && Value.IndexOf('.') > -1;

}

public class Port
{
    public Port(int value) { Value = value; }
    public int Value { get; }

    public static readonly Port Any = new Port(-1); // Assuming -1 signifies any port

    public static Port Create(int portNumber) => new Port(portNumber);
}

public class Path
{
    public Path(string value) { Value = value; }
    public string Value { get; }

    public static readonly Path Any = new Path("*");

    public static Path Create(string path) => new Path(path);
}

public class Query
{
    public Query(string value) { Value = value; }
    public string Value { get; }

    public static readonly Query Any = new Query("*");

    public static Query Create(string query) => new Query(query);
}

public class Fragment
{
    public Fragment(string value) { Value = value; }
    public string Value { get; }

    public static readonly Fragment Any = new Fragment("*");

    public static Fragment Create(string fragment) => new Fragment(fragment);
}


public class RedirectValidition
{
    static RedirectValidition()
    {
        var provider = new ServiceCollection()
            .AddLogging()
            .AddSingleton<IRedirectService, RedirectService>()
            .AddSanitizers()
            .BuildServiceProvider();
        ServiceProvider = provider;

    }

    public static ServiceProvider ServiceProvider { get; }
}

public static class RedirectServiceExtensions
{

    public static IServiceCollection AddAllowAnyRedirectService(this IServiceCollection services)
    {
        services.AddSingleton<AllowAnyRedirectService>();
        return services;
    }
}

public class AllowAnyRedirectService : RedirectService
{
    public AllowAnyRedirectService(ILogger<RedirectService> logger, ISanitizer sanitizer) : base(logger, sanitizer)
    {
        AddRedirectRule(RedirectRule.AllowAny);
    }
}

public class RedirectService : IRedirectService
{
    public RedirectService(ILogger<RedirectService> logger, ISanitizer sanitizer)
    {
        _logger = logger;
        _sanitizer = sanitizer;
    }
    private readonly List<RedirectRule> _rules = new List<RedirectRule>();
    private readonly ILogger<RedirectService> _logger;
    private readonly ISanitizer _sanitizer;

    public IRedirectService AddRedirectRule(RedirectRule rule)
    {
        _rules.Add(rule);
        return this;
    }
    public IRedirectService ClearRules()
    {
        _rules.Clear();
        return this;
    }

    public IRedirectService AddRule(RedirectRule rule)
    {
        _rules.Add(rule);
        return this;
    }

    public IRedirectService RemoveRule(RedirectRule rule)
    {
        _rules.Remove(rule);
        return this;
    }
    public IRedirectService AddRules(IEnumerable<RedirectRule> rules)
    {
        _rules.AddRange(rules);
        return this;
    }
    public IRedirectService RemoveRules(IEnumerable<RedirectRule> rules)
    {
        foreach (var rule in rules)
        {
            RemoveRule(rule);
        }
        return this;
    }

    public virtual bool IsRedirectAllowed(string redirectUrl)
    {
        if (!Uri.TryCreate(redirectUrl, UriKind.RelativeOrAbsolute, out var uri))
        {
            _logger.LogWarning("Invalid URL: {redirectUrl}", redirectUrl.SanitizeForLog());
            return false;
        }

        // If the URL is relative, assume it's allowed, or handle according to your policy
        if (!uri.IsAbsoluteUri)
        {
            // Handle relative URLs based on your specific requirements.
            // For example, we might always allow them, check them against a specific set of rules,
            // or reject them outright.
            return HandleRelativeUrl(uri);
        }

        foreach (var rule in _rules)
        {
            if (IsRuleMatch(uri, rule))
            {
                return true;
            }
        }

        return false;
    }

    public bool HandleRelativeUrl(Uri uri)
    {
        // Implement logic for handling relative URLs on local server.
        // This is a placeholder as restricting redirect URLs on the local server is not a common scenario.
        // For now: return true to allow all relative URLs
        return true;
    }


    public bool IsRuleMatch(Uri uri, RedirectRule rule)
    {
        return IsSchemeMatch(uri, rule.AllowedScheme) &&
               IsHostMatch(uri, rule.AllowedHost) &&
               IsPortMatch(uri, rule.AllowedPort) &&
               IsPathMatch(uri, rule.AllowedPath) &&
               IsQueryMatch(uri, rule.AllowedQuery) &&
               IsFragmentMatch(uri, rule.AllowedFragment);
    }

    public bool IsSchemeMatch(Uri uri, Scheme allowedScheme)
    {
        return allowedScheme == Scheme.Any || uri.Scheme.Equals(allowedScheme.Name, StringComparison.OrdinalIgnoreCase);
    }

    public bool IsHostMatch(string url, Host allowedHost)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            return false;
        }

        if (allowedHost == Host.Any)
        {
            return true;
        }

        // Check if URL already has a valid scheme; if not, prepend "http://" as a default
        if (!url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) &&
            !url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
        {
            url = "http://" + url;
        }

        // Attempt to parse the URL into a Uri object
        if (Uri.TryCreate(url, UriKind.Absolute, out var uri))
        {
            // Delegate to the existing IsHostMatch method that takes a Uri object
            return IsHostMatch(uri, allowedHost);
        }
        else
        {
            // Log error or handle invalid URL format as needed
            return false;
        }
    }


    public bool IsHostMatch(Uri uri, Host allowedHost)
    {
        // Handle the case where any host is allowed
        if (allowedHost == Host.Any)
        {
            return true;
        }

        string uriHost = uri.Host;
        string allowedHostValue = allowedHost.Value;

        // Direct match or wildcard match
        if (uriHost.Equals(allowedHostValue, StringComparison.OrdinalIgnoreCase) ||
            allowedHostValue == "*")
        {
            return true;
        }

        // Check for wildcard subdomain match
        if (allowedHostValue.StartsWith("*."))
        {
            string allowedDomain = allowedHostValue.Substring(2);
            if (uriHost.EndsWith(allowedDomain, StringComparison.OrdinalIgnoreCase) &&
                // Additional check to ensure we're matching subdomains and not just any part of the host
                (uriHost.Count(c => c == '.') == allowedDomain.Count(c => c == '.') + 1))
            {
                return true;
            }
        }

        return false;
    }

    public bool IsPortMatch(Uri uri, Port allowedPort)
    {
        return allowedPort == Port.Any || uri.Port == allowedPort.Value;
    }

    public bool IsPathMatch(Uri uri, Path allowedPath)
    {
        return allowedPath == Path.Any || uri.AbsolutePath.Equals(allowedPath.Value, StringComparison.OrdinalIgnoreCase);
    }

    public bool IsQueryMatch(Uri uri, Query allowedQuery)
    {
        return allowedQuery == Query.Any || uri.Query.Equals(allowedQuery.Value, StringComparison.OrdinalIgnoreCase);
    }

    public bool IsFragmentMatch(Uri uri, Fragment allowedFragment)
    {
        return allowedFragment == Fragment.Any || uri.Fragment.Equals(allowedFragment.Value, StringComparison.OrdinalIgnoreCase);
    }

}


public class RedirectUrlValidator
{
    private readonly IRedirectService _redirectService;

    public RedirectUrlValidator(IRedirectService redirectService)
    {
        _redirectService = redirectService;
    }

    public bool IsRedirectUrlValid(string redirectUrl)
    {
        return _redirectService.IsRedirectAllowed(redirectUrl);
    }
}
