/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

namespace IdentityServer8.EntityFramework.Options
{
    /// <summary>
    /// Class to control a table's name and schema.
    /// </summary>
    public class TableConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TableConfiguration"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public TableConfiguration(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TableConfiguration"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="schema">The schema.</param>
        public TableConfiguration(string name, string schema)
        {
            Name = name;
            Schema = schema;
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the schema.
        /// </summary>
        /// <value>
        /// The schema.
        /// </value>
        public string Schema { get; set; }
    }
}
