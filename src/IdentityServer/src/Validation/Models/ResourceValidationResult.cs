/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

namespace IdentityServer8.Validation;

/// <summary>
/// Result of validation of requested scopes and resource indicators.
/// </summary>
public class ResourceValidationResult
{
    /// <summary>
    /// Ctor
    /// </summary>
    public ResourceValidationResult()
    {
    }

    /// <summary>
    /// Ctor
    /// </summary>
    /// <param name="resources"></param>
    public ResourceValidationResult(Resources resources)
    {
        Resources = resources;
        ParsedScopes = resources.ToScopeNames().Select(x => new ParsedScopeValue(x)).ToList();
    }

    /// <summary>
    /// Ctor
    /// </summary>
    /// <param name="resources"></param>
    /// <param name="parsedScopeValues"></param>
    public ResourceValidationResult(Resources resources, IEnumerable<ParsedScopeValue> parsedScopeValues)
    {
        Resources = resources;
        ParsedScopes = parsedScopeValues.ToList();
    }

    /// <summary>
    /// Indicates if the result was successful.
    /// </summary>
    public bool Succeeded => ParsedScopes.Any() && !InvalidScopes.Any();

    /// <summary>
    /// The resources of the result.
    /// </summary>
    public Resources Resources { get; set; } = new Resources();

    /// <summary>
    /// The parsed scopes represented by the result.
    /// </summary>
    public ICollection<ParsedScopeValue> ParsedScopes { get; set; } = new HashSet<ParsedScopeValue>();

    /// <summary>
    /// The original (raw) scope values represented by the validated result.
    /// </summary>
    public IEnumerable<string> RawScopeValues => ParsedScopes.Select(x => x.RawValue);

    /// <summary>
    /// The requested scopes that are invalid.
    /// </summary>
    public ICollection<string> InvalidScopes { get; set; } = new HashSet<string>();

    /// <summary>
    /// Returns new result filted by the scope values.
    /// </summary>
    /// <param name="scopeValues"></param>
    /// <returns></returns>
    public ResourceValidationResult Filter(IEnumerable<string> scopeValues)
    {
        scopeValues ??= Enumerable.Empty<string>();

        var offline = scopeValues.Contains(IdentityServerConstants.StandardScopes.OfflineAccess);

        var parsedScopesToKeep = ParsedScopes.Where(x => scopeValues.Contains(x.RawValue)).ToArray();
        var parsedScopeNamesToKeep = parsedScopesToKeep.Select(x => x.ParsedName).ToArray();

        var identityToKeep = Resources.IdentityResources.Where(x => parsedScopeNamesToKeep.Contains(x.Name));
        var apiScopesToKeep = Resources.ApiScopes.Where(x => parsedScopeNamesToKeep.Contains(x.Name));

        var apiScopesNamesToKeep = apiScopesToKeep.Select(x => x.Name).ToArray();
        var apiResourcesToKeep = Resources.ApiResources.Where(x => x.Scopes.Any(y => apiScopesNamesToKeep.Contains(y)));

        var resources = new Resources(identityToKeep, apiResourcesToKeep, apiScopesToKeep)
        {
            OfflineAccess = offline
        };
        
        return new ResourceValidationResult()
        {
            Resources = resources,
            ParsedScopes = parsedScopesToKeep
        };
    }
}
