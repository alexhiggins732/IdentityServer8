/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

using System.IO.Pipes;

class CallbackManager
{
    private readonly string _name;

    public CallbackManager(string name)
    {
        _name = name ?? throw new ArgumentNullException(nameof(name));
    }

    public int ClientConnectTimeoutSeconds { get; set; } = 1;

    public async Task RunClient(string args)
    {
        using (var client = new NamedPipeClientStream(".", _name, PipeDirection.Out))
        {
            await client.ConnectAsync(ClientConnectTimeoutSeconds * 1000);

            using (var sw = new StreamWriter(client) { AutoFlush = true })
            {
                await sw.WriteAsync(args);
            }
        }
    }

    public async Task<string> RunServer(CancellationToken? token = null)
    {
        token = CancellationToken.None;

        using (var server = new NamedPipeServerStream(_name, PipeDirection.In))
        {
            await server.WaitForConnectionAsync(token.Value);

            using (var sr = new StreamReader(server))
            {
                var msg = await sr.ReadToEndAsync();
                return msg;
            }
        }
    }
}
