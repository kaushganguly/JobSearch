using Microsoft.Extensions.FileProviders;
using NYCJobsWeb;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });
builder.Services.AddSingleton<JobsSearch>();

var app = builder.Build();

UseStaticFolder(app, "Content", "/content");
UseStaticFolder(app, "Scripts", "/scripts");
UseStaticFolder(app, "Images", "/images");
UseStaticFolder(app, "Fonts", "/fonts");

app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

static void UseStaticFolder(WebApplication app, string folderName, string requestPath)
{
    var physicalPath = Path.Combine(app.Environment.ContentRootPath, folderName);
    if (!Directory.Exists(physicalPath))
    {
        return;
    }

    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(physicalPath),
        RequestPath = requestPath
    });
}
