/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

namespace IdentityServer.IntegrationTests.Clients.Setup;

public class CustomResponseDto
{
    public string string_value { get; set; }
    public int int_value { get; set; }

    public CustomResponseDto nested { get; set; }

    public static CustomResponseDto Create
    {
        get
        {
            return new CustomResponseDto
            {
                string_value = "dto_string",
                int_value = 43,
                nested = new CustomResponseDto
                {
                    string_value = "dto_nested_string",
                    int_value = 44
                }
            };
        }
    }
}
