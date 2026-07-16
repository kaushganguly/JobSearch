using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace AzureSearchBackupRestore;

public static class AzureSearchHelper
{
    public const string ApiVersionString = "api-version=2015-02-28-Preview";

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };

    public static string SerializeJson(object value) => JsonSerializer.Serialize(value, JsonOptions);

    public static T? DeserializeJson<T>(string json) => JsonSerializer.Deserialize<T>(json, JsonOptions);

    public static async Task<HttpResponseMessage> SendSearchRequestAsync(HttpClient client, HttpMethod method, Uri uri, string? json = null)
    {
        var builder = new UriBuilder(uri);
        var separator = string.IsNullOrWhiteSpace(builder.Query) ? string.Empty : "&";
        builder.Query = builder.Query.TrimStart('?') + separator + ApiVersionString;

        using var request = new HttpRequestMessage(method, builder.Uri);
        if (json is not null)
        {
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
        }

        return await client.SendAsync(request);
    }
}
