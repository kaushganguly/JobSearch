using System.Net.Http;

namespace AzureSearchBackupRestore;

internal static class Program
{
    private static readonly HttpClient HttpClient = new();
    private static Uri _serviceUri = null!;
    private static string _dataDirectory = null!;

    private static void Main()
    {
        var serviceName = Environment.GetEnvironmentVariable("TARGET_SEARCH_SERVICE_NAME");
        var apiKey = Environment.GetEnvironmentVariable("TARGET_SEARCH_SERVICE_API_KEY");
        if (string.IsNullOrWhiteSpace(serviceName) || string.IsNullOrWhiteSpace(apiKey))
        {
            Console.Error.WriteLine(
                "Set TARGET_SEARCH_SERVICE_NAME and TARGET_SEARCH_SERVICE_API_KEY before running the data loader.");
            Environment.ExitCode = 1;
            return;
        }

        try
        {
            _serviceUri = new Uri($"https://{serviceName}.search.windows.net");
            _dataDirectory = FindDataDirectory();
            HttpClient.DefaultRequestHeaders.Add("api-key", apiKey);

            LaunchImportProcess("zipcodes");
            LaunchImportProcess("nycjobs");

            Console.WriteLine("Import requests completed.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
            Environment.ExitCode = 1;
        }
    }

    private static void LaunchImportProcess(string indexName)
    {
        Console.WriteLine($"Deleting {indexName} index...");
        DeleteIndex(indexName);
        Console.WriteLine($"Creating {indexName} index...");
        CreateTargetIndex(indexName);
        Console.WriteLine($"Uploading data to {indexName}...");
        ImportFromJson(indexName);
    }

    private static void DeleteIndex(string indexName)
    {
        var uri = new Uri(_serviceUri, $"/indexes/{indexName}");
        using var response = AzureSearchHelper.SendSearchRequest(HttpClient, HttpMethod.Delete, uri);
        if (response.StatusCode != System.Net.HttpStatusCode.NotFound)
        {
            response.EnsureSuccessStatusCode();
        }
    }

    private static void CreateTargetIndex(string indexName)
    {
        var schemaPath = Path.Combine(_dataDirectory, $"{indexName}.schema");
        var json = File.ReadAllText(schemaPath);
        var uri = new Uri(_serviceUri, "/indexes");
        using var response = AzureSearchHelper.SendSearchRequest(HttpClient, HttpMethod.Post, uri, json);
        response.EnsureSuccessStatusCode();
    }

    private static void ImportFromJson(string indexName)
    {
        var files = Directory.GetFiles(_dataDirectory, $"{indexName}*.json")
            .OrderBy(file => file, StringComparer.Ordinal);
        foreach (var fileName in files)
        {
            Console.WriteLine($"Uploading documents from file {Path.GetFileName(fileName)}");
            var json = File.ReadAllText(fileName);
            var uri = new Uri(_serviceUri, $"/indexes/{indexName}/docs/index");
            using var response = AzureSearchHelper.SendSearchRequest(HttpClient, HttpMethod.Post, uri, json);
            response.EnsureSuccessStatusCode();
        }
    }

    private static string FindDataDirectory()
    {
        foreach (var start in new[] { Directory.GetCurrentDirectory(), AppContext.BaseDirectory })
        {
            var current = new DirectoryInfo(start);
            while (current is not null)
            {
                var candidate = Path.Combine(current.FullName, "NYCJobsWeb", "Schema_and_Data");
                if (Directory.Exists(candidate))
                {
                    return candidate;
                }

                current = current.Parent;
            }
        }

        throw new DirectoryNotFoundException(
            "Could not find NYCJobsWeb/Schema_and_Data. Run the data loader from the repository or a build output beneath it.");
    }
}
