namespace Microsoft.DependencyInjection.Extensions;

public class Ioc
{
    static Ioc()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddSanitizers();
        services.AddAllowAnyRedirectService();
        services.AddSingleton<IRedirectService, AllowAnyRedirectService>();
        ServiceProvider = services.BuildServiceProvider();
        var sanitizer = ServiceProvider.GetRequiredService<ISanitizer>();
        Sanitizer = sanitizer;
        var redirectService = ServiceProvider.GetRequiredService<IRedirectService>();
        RedirectService = redirectService;
    }

    public static ServiceProvider ServiceProvider { get; }
    public static ISanitizer Sanitizer { get; }
    public static IRedirectService RedirectService { get; }

}
