
JwtSecurityTokenHandler.DefaultMapInboundClaims = false;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultScheme = "Cookies";
        options.DefaultChallengeScheme = "oidc";
    })
    .AddCookie("Cookies")
    .AddOpenIdConnect("oidc", options =>
    {
        options.Authority = "https://localhost:5001";

        options.ClientId = "mvc";
        options.ClientSecret = "secret";
        options.ResponseType = "code";

        options.Scope.Add("api1");

        options.SaveTokens = true;
    });


using (var app = builder.Build())
{
    if (app.Environment.IsDevelopment())
        app.UseDeveloperExceptionPage();
    else
        app.UseExceptionHandler("/Home/Error");

    app.UseStaticFiles();
    app.UseRouting();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapDefaultControllerRoute().RequireAuthorization();

    await app.RunAsync();
}
