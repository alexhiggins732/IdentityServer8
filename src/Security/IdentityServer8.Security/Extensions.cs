using IdentityServer8.Security;
using Microsoft.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Microsoft.DependencyInjection.Extensions
{
    public class Ioc
    {
        static Ioc()
        {
            var services = new ServiceCollection();
            services.AddSanitizers();
            ServiceProvider = services.BuildServiceProvider();
            var sanitizer = ServiceProvider.GetRequiredService<ISanitizer>();
            Sanitizer = sanitizer;
        }

        public static ServiceProvider ServiceProvider { get; }
        public static ISanitizer Sanitizer { get; }
    }
}
