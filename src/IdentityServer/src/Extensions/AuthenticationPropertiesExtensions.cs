/*
 Copyright (c) 2024, Martijn van Put - https://github.com/mvput/ 

 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

namespace IdentityServer.Extensions;

/// <summary>
/// Extensions for AuthenticationProperties
/// </summary>
public static class AuthenticationPropertiesExtensions
{
    internal const string SessionIdKey = "session_id";
    internal const string ClientListKey = "client_list";

    /// <summary>
    /// Gets the user's session identifier.
    /// </summary>
    /// <param name="properties"></param>
    /// <returns></returns>
    public static string GetSessionId(this AuthenticationProperties properties)
    {
        if (properties?.Items.ContainsKey(SessionIdKey) == true)
        {
            return properties.Items[SessionIdKey];
        }

        return null;
    }

    /// <summary>
    /// Sets the user's session identifier.
    /// </summary>
    /// <param name="properties"></param>
    /// <param name="sid">The session id</param>
    /// <returns></returns>
    public static void SetSessionId(this AuthenticationProperties properties, string sid)
    {
        properties.Items[SessionIdKey] = sid;
    }

    /// <summary>
    /// Gets the list of client ids the user has signed into during their session.
    /// </summary>
    /// <param name="properties"></param>
    /// <returns></returns>
    public static IEnumerable<string> GetClientList(this AuthenticationProperties properties)
    {
        if (properties?.Items.ContainsKey(ClientListKey) == true)
        {
            var value = properties.Items[ClientListKey];
            return DecodeList(value);
        }

        return Enumerable.Empty<string>();
    }

    /// <summary>
    /// Removes the list of client ids.
    /// </summary>
    /// <param name="properties"></param>
    public static void RemoveClientList(this AuthenticationProperties properties)
    {
        properties?.Items.Remove(ClientListKey);
    }

    /// <summary>
    /// Adds a client to the list of clients the user has signed into during their session.
    /// </summary>
    /// <param name="properties"></param>
    /// <param name="clientId"></param>
    public static void AddClientId(this AuthenticationProperties properties, string clientId)
    {
        if (clientId == null) throw new ArgumentNullException(nameof(clientId));

        var clients = properties.GetClientList();
        if (!clients.Contains(clientId))
        {
            var update = clients.ToList();
            update.Add(clientId);

            var value = EncodeList(update);
            if (value == null)
            {
                properties.Items.Remove(ClientListKey);
            }
            else
            {
                properties.Items[ClientListKey] = value;
            }
        }
    }


    private static IEnumerable<string> DecodeList(string value)
    {
        if (value.IsPresent())
        {
            var bytes = Base64Url.Decode(value);
            value = Encoding.UTF8.GetString(bytes);
            return ObjectSerializer.FromString<string[]>(value);
        }

        return Enumerable.Empty<string>();
    }

    private static string EncodeList(IEnumerable<string> list)
    {
        if (list != null && list.Any())
        {
            var value = ObjectSerializer.ToString(list);
            var bytes = Encoding.UTF8.GetBytes(value);
            value = Base64Url.Encode(bytes);
            return value;
        }

        return null;
    }
}
