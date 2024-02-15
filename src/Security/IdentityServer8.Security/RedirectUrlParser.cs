/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

namespace IdentityServer8.Security;

public readonly ref struct RedirectUrlS
{
    private readonly ReadOnlySpan<char> _input;

    public RedirectUrlS(string input)
    {
        _input = input.AsSpan();
    }

    //public RedirectUrlS Parse()
    //{
    //    // Default values representing wildcards
    //    var scheme = Scheme.Any;
    //    var host = Host.Any;
    //    var port = Port.Any;
    //    var path = Path.Any;
    //    var query = Query.Any;
    //    var fragment = Fragment.Any;
    //    int offset = 0;
    //    int schemeEnd = _input.IndexOf("://");
    //    if (schemeEnd != -1)
    //    {
    //        scheme = new Scheme(_input.Slice(0, schemeEnd).ToString());
    //        offset = schemeEnd + 3;
    //    }

    //    int pathStart = _input.IndexOf('/');
    //    int portEnd = _input.Slice(0, pathStart != -1 ? pathStart : _input.Length).IndexOf(':');

    //    if (portEnd != -1)
    //    {
    //        host = new Host(_input.Slice(0, portEnd).ToString());
    //        port = new Port(int.Parse(_input.Slice(portEnd + 1, pathStart - portEnd - 1).ToString()));
    //    }
    //    else if (pathStart != -1)
    //    {
    //        host = new Host(_input.Slice(0, pathStart).ToString());
    //    }
    //    else
    //    {
    //        host = new Host(_input.ToString());
    //    }

    //    if (pathStart != -1)
    //    {
    //        offset = pathStart;
    //        int queryStart = _input.IndexOf('?');
    //        int fragmentStart = _input.IndexOf('#');

    //        if (queryStart != -1)
    //        {
    //            path = new Path(_input.Slice(0, queryStart).ToString());
    //            offset = queryStart + 1;


    //            if (fragmentStart != -1)
    //            {
    //                query = new Query(_input.Slice(0, fragmentStart - queryStart - 1).ToString());
    //                fragment = new Fragment(_input.Slice(fragmentStart + 1).ToString());
    //            }
    //            else
    //            {
    //                query = new Query(_input.ToString());
    //            }
    //        }
    //        else if (fragmentStart != -1)
    //        {
    //            path = new Path(_input.Slice(0, fragmentStart).ToString());
    //            fragment = new Fragment(_input.Slice(fragmentStart + 1).ToString());
    //        }
    //        else
    //        {
    //            path = new Path(_input.ToString());
    //        }
    //    }

    //    return new RedirectUrl
    //    {
    //        Scheme = scheme,
    //        Host = host,
    //        Port = port,
    //        Path = path,
    //        Query = query,
    //        Fragment = fragment
    //    };
    //}
}

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
        int portNumber = 0;
        if(int.TryParse(port, out var parsedPort))
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
