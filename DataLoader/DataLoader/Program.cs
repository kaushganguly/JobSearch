using System.Net.Http;
using System.Text.Json;

namespace AzureSearchBackupRestore;

internal static class Program
{
    private static readonly HttpClient HttpClient = new();

    private sealed record LoaderSettings(string TargetSearchServiceName, string TargetSearchServiceApiKey);

    private static async Task Main(string[] args)
    {
        try
        {
            var settings = LoadSettings();
            var serviceUri = new Uri($"https://{settings.TargetSearchServiceName}.search.windows.net");
            var schemaDataDirectory = ResolveSchemaAndDataDirectory();

            HttpClient.DefaultRequestHeaders.Remove("api-key");
            HttpClient.DefaultRequestHeaders.Add("api-key", settings.TargetSearchServiceApiKey);

            await LaunchImportProcessAsync(serviceUri, schemaDataDirectory, "zipcodes");
            await LaunchImportProcessAsync(serviceUri, schemaDataDirectory, "nycjobs");

            Console.WriteLine("NOTE: For really large indexes it may take some time to index all content.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            Console.WriteLine("Did you remember to set TargetSearchServiceName and TargetSearchServiceApiKey in DataLoader/DataLoader/appsettings.json?");
            Environment.ExitCode = 1;
        }
    }

    private static LoaderSettings LoadSettings()
    {
        var configPath = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
        if (!File.Exists(configPath))
        {
            throw new FileNotFoundException($"Configuration file not found: {configPath}");
        }

        using var document = JsonDocument.Parse(File.ReadAllText(configPath));
        var root = document.RootElement;

        var serviceName = Environment.GetEnvironmentVariable("TARGET_SEARCH_SERVICE_NAME")
            ?? root.GetProperty("TargetSearchServiceName").GetString();
        var apiKey = Environment.GetEnvironmentVariable("TARGET_SEARCH_SERVICE_API_KEY")
            ?? root.GetProperty("TargetSearchServiceApiKey").GetString();

        if (string.IsNullOrWhiteSpace(serviceName) || serviceName.Contains('[', StringComparison.Ordinal))
        {
            throw new InvalidOperationException("TargetSearchServiceName is not configured.");
        }

        if (string.IsNullOrWhiteSpace(apiKey) || apiKey.Contains('[', StringComparison.Ordinal))
        {
            throw new InvalidOperationException("TargetSearchServiceApiKey is not configured.");
        }

        return new LoaderSettings(serviceName, apiKey);
    }

    private static async Task LaunchImportProcessAsync(Uri serviceUri, string schemaDataDirectory, string indexName)
    {
        Console.WriteLine($"Deleting {indexName} index...");
        await DeleteIndexAsync(serviceUri, indexName);
        Console.WriteLine($"Creating {indexName} index...");
        await CreateTargetIndexAsync(serviceUri, schemaDataDirectory, indexName);
        Console.WriteLine($"Uploading data to {indexName}...");
        await ImportFromJsonAsync(serviceUri, schemaDataDirectory, indexName);
    }

    private static async Task DeleteIndexAsync(Uri serviceUri, string indexName)
    {
        try
        {
            var uri = new Uri(serviceUri, $"/indexes/{indexName}");
            using var response = await AzureSearchHelper.SendSearchRequestAsync(HttpClient, HttpMethod.Delete, uri);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Delete returned {(int)response.StatusCode} ({response.ReasonPhrase}) for index {indexName}.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting index: {ex.Message}");
        }
    }

    private static async Task CreateTargetIndexAsync(Uri serviceUri, string schemaDataDirectory, string indexName)
    {
        try
        {
            var schemaPath = Path.Combine(schemaDataDirectory, $"{indexName}.schema");
            var json = await File.ReadAllTextAsync(schemaPath);
            var uri = new Uri(serviceUri, "/indexes");
            using var response = await AzureSearchHelper.SendSearchRequestAsync(HttpClient, HttpMethod.Post, uri, json);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating index: {ex.Message}");
        }
    }

    private static async Task ImportFromJsonAsync(Uri serviceUri, string schemaDataDirectory, string indexName)
    {
        try
        {
            foreach (var fileName in Directory.GetFiles(schemaDataDirectory, $"{indexName}*.json"))
            {
                Console.WriteLine($"Uploading documents from file {Path.GetFileName(fileName)}");
                var json = await File.ReadAllTextAsync(fileName);
                var uri = new Uri(serviceUri, $"/indexes/{indexName}/docs/index");
                using var response = await AzureSearchHelper.SendSearchRequestAsync(HttpClient, HttpMethod.Post, uri, json);
                response.EnsureSuccessStatusCode();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error uploading documents: {ex.Message}");
        }
    }

    private static string ResolveSchemaAndDataDirectory()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);

        while (directory is not null)
        {
            var candidate = Path.Combine(directory.FullName, "NYCJobsWeb", "Schema_and_Data");
            if (Directory.Exists(candidate))
            {
                return candidate;
            }

            directory = directory.Parent;
        }

        throw new DirectoryNotFoundException("Unable to locate NYCJobsWeb/Schema_and_Data from the application output directory.");
    }
}
