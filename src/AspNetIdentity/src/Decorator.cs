/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

namespace IdentityServer8.AspNetIdentity;

internal class Decorator<TService>
{
    public TService Instance { get; set; }

    public Decorator(TService instance)
    {
        Instance = instance;
    }
}

internal class Decorator<TService, TImpl> : Decorator<TService>
    where TImpl : class, TService
{
    public Decorator(TImpl instance) : base(instance)
    {
    }
}

internal class DisposableDecorator<TService> : Decorator<TService>, IDisposable
{
    public DisposableDecorator(TService instance) : base(instance)
    {
    }

    public void Dispose()
    {
        (Instance as IDisposable)?.Dispose();
    }
}
