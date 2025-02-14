/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

using System.Diagnostics.CodeAnalysis;

namespace IdentityServer8.Security;


[ExcludeFromCodeCoverage(Justification = "Class is currently unused")]
public class RedirectUrlParser
{
    public static UrlParts Parse(string redirectUrl)
    {
        // extract scheme, host, port, path, query, and fragment from the redirectUrl
        // and return a RedirectUrl object
        // expand: example.com to *://**.example.com:*/**?**#*
        // expand: example.com/path to *://**.example.com:*/path?**#*
        // expand: example.com/* to *://**.example.com:*/**?**#*
        // expand: example.com/path/* to *://**.example.com:*/path/*?**#*
        // expand: example.com/path/** to *://**.example.com:*/path/**?**#*
        // expand: example.com/path/1/* to *://**.example.com:*/path/1/*?query#fragment
        // expand: example.com/path/1/** to *://**.example.com:*/path/1/**?query#fragment
        // expand: example.com/path/** to *://**.example.com:*/path/**?**#*
        // expand: example.com/path/1/path/2?quru to *://**.example.com:*/path?query#fragment

        var schemeParts = redirectUrl.Split("://");
        var originalScheme = schemeParts.Length == 1 ? "" : schemeParts[0];
        var scheme = schemeParts.Length == 1 ? Scheme.Any : Scheme.Parse(schemeParts[0]);


        var tail = redirectUrl.Substring(schemeParts.Length == 1 ? 0 : schemeParts[0].Length + 3);
        var idx = tail.IndexOf("/");
        var hostAndPort = tail.Substring(0, idx);
        var portDelimiter = hostAndPort.IndexOf(":");
        var host = portDelimiter > 0 ? hostAndPort.Substring(0, portDelimiter) : hostAndPort;
        var port = portDelimiter > 0 ? hostAndPort.Substring(portDelimiter + 1) : "";
        ushort portNumber = 0;
        if(ushort.TryParse(port, out var parsedPort))
        {
            portNumber = parsedPort;
        }
        else
        {
            //TODO: Modify port to accept wildcards, multiple ports, and ranges
            //port = "";
        }
        var domainParts = host.Split(".");

        var absolutePath = idx > -1 ? tail.Substring(idx) : "";
        var pathParts = absolutePath.Split("?");
        var path = pathParts[0];
        var query = pathParts.Length == 1 ? "" : pathParts[1];
        var queryParts = query.Split("#");
        var queryString = queryParts[0];
        var queryStringParts = queryString.Split("&");

        var fragment = queryParts.Length == 1 ? "" : queryParts[1];
        var fragmentParts = fragment.Split("#");

        return new UrlParts
        {

            Scheme = scheme,
            Host = Host.Create(host),
            Port = Port.Create(portNumber),
            Path = Path.Create(path),
            Query = Query.Create(queryString),
            Fragment = Fragment.Create(fragment)
        };

    }

}


[ExcludeFromCodeCoverage(Justification = "Class is currently unused")]
public class UrlParts
{
    public UrlParts()
    {
        Scheme= Scheme.Any;
        Host = Host.Any;
        Port = Port.Any;
        Path = Path.Any;
        Query = Query.Any;
        Fragment = Fragment.Any;

    }
    public UrlParts(Scheme scheme, Host host, Port port, Path path, Query query, Fragment fragment)
    {
        Scheme = scheme;
        Host = host;
        Port = port;
        Path = path;
        Query = query;
        Fragment = fragment;
    }
    public Scheme Scheme { get; set; }
    public Host Host { get; set; }
    public Port Port { get; set; }
    public Path Path { get; set; }
    public Query Query { get; set; }
    public Fragment Fragment { get; set; }
}
