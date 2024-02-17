/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

using IdentityServer8.Models;

namespace IdentityServer8.EntityFramework.Mappers
{
    /// <summary>
    /// Extension methods to map to/from entity/model for persisted grants.
    /// </summary>
    public static class PersistedGrantMappers
    {
        static PersistedGrantMappers()
        {
            Mapper = new MapperConfiguration(cfg =>cfg.AddProfile<PersistedGrantMapperProfile>())
                .CreateMapper();
        }

        internal static IMapper Mapper { get; }

        /// <summary>
        /// Maps an entity to a model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static PersistedGrant ToModel(this Entities.PersistedGrant entity)
        {
            return entity == null ? null : Mapper.Map<PersistedGrant>(entity);
        }

        /// <summary>
        /// Maps a model to an entity.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static Entities.PersistedGrant ToEntity(this PersistedGrant model)
        {
            return model == null ? null : Mapper.Map<Entities.PersistedGrant>(model);
        }

        /// <summary>
        /// Updates an entity from a model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="entity">The entity.</param>
        public static void UpdateEntity(this PersistedGrant model, Entities.PersistedGrant entity)
        {
            Mapper.Map(model, entity);
        }
    }
}
