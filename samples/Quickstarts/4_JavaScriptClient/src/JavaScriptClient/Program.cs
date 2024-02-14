var app = WebApplication.Create(args);
app.UseDefaultFiles();
app.UseStaticFiles();
await app.RunAsync();