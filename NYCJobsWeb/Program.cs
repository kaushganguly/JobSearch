using Microsoft.Extensions.FileProviders;
using NYCJobsWeb;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
        options.JsonSerializerOptions.DictionaryKeyPolicy = null;
    });

builder.Services.AddSingleton<JobsSearch>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

MapStaticFolder(app, "Content", "/Content", "/content");
MapStaticFolder(app, "Scripts", "/Scripts", "/scripts");
MapStaticFolder(app, "Images", "/Images", "/images");
MapStaticFolder(app, "Fonts", "/Fonts", "/fonts");

app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

static void MapStaticFolder(WebApplication app, string folderName, params string[] requestPaths)
{
    var folderPath = Path.Combine(app.Environment.ContentRootPath, folderName);
    if (!Directory.Exists(folderPath))
    {
        return;
    }

    foreach (var requestPath in requestPaths.Distinct(StringComparer.Ordinal))
    {
        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(folderPath),
            RequestPath = requestPath
        });
    }
}
