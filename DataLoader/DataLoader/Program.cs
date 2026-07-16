// This is a prototype tool that allows for import of sample data to an Azure Search index

using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Net.Http;

namespace AzureSearchBackupRestore
{
    class Program
    {
        private static readonly IConfigurationRoot Configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        private static readonly string TargetSearchServiceName = Configuration["TargetSearchServiceName"];
        private static readonly string TargetSearchServiceApiKey = Configuration["TargetSearchServiceApiKey"];

        private static HttpClient HttpClient;
        private static Uri ServiceUri;
        private static readonly string SchemaAndDataDirectory = ResolveSchemaAndDataPath();

        static void Main(string[] args)
        {
            try
            {
                ServiceUri = new Uri("https://" + TargetSearchServiceName + ".search.windows.net");
                HttpClient = new HttpClient();
                HttpClient.DefaultRequestHeaders.Add("api-key", TargetSearchServiceApiKey);

                LaunchImportProcess("zipcodes");
                LaunchImportProcess("nycjobs");

                Console.WriteLine("NOTE: For really large indexes it may take some time to index all content.\r\n");
                Console.WriteLine("Press any key to continue.\r\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", ex.Message);
                Console.WriteLine("Did you remember to set your TargetSearchServiceName and TargetSearchServiceApiKey in appsettings.json?\r\n");
            }
            Console.ReadLine();
        }

        private static string ResolveSchemaAndDataPath()
        {
            var localPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "NYCJobsWeb", "Schema_and_Data"));
            if (Directory.Exists(localPath))
            {
                return localPath;
            }

            var repoRelativePath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "NYCJobsWeb", "Schema_and_Data"));
            if (Directory.Exists(repoRelativePath))
            {
                return repoRelativePath;
            }

            throw new DirectoryNotFoundException("Could not locate NYCJobsWeb/Schema_and_Data.");
        }

        private static void LaunchImportProcess(string indexName)
        {
            Console.WriteLine("Deleting " + indexName + " index...");
            DeleteIndex(indexName);
            Console.WriteLine("Creating " + indexName + " index...");
            CreateTargetIndex(indexName);
            Console.WriteLine("Uploading data to " + indexName + "...");
            ImportFromJSON(indexName);
        }

        private static void DeleteIndex(string indexName)
        {
            try
            {
                try
                {
                    Uri uri = new Uri(ServiceUri, "/indexes/" + indexName);
                    HttpResponseMessage response = AzureSearchHelper.SendSearchRequest(HttpClient, HttpMethod.Delete, uri);
                    response.EnsureSuccessStatusCode();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: {0}", ex.Message);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error deleting index: {0}\r\n", ex.Message);
            }
        }

        static void CreateTargetIndex(string indexName)
        {
            string json = File.ReadAllText(Path.Combine(SchemaAndDataDirectory, indexName + ".schema"));
            try
            {
                Uri uri = new Uri(ServiceUri, "/indexes");
                HttpResponseMessage response = AzureSearchHelper.SendSearchRequest(HttpClient, HttpMethod.Post, uri, json);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", ex.Message);
            }

        }

        static void ImportFromJSON(string indexName)
        {
            try
            {
                foreach (string fileName in Directory.GetFiles(SchemaAndDataDirectory, indexName + "*.json"))
                {
                    Console.WriteLine("Uploading documents from file {0}", fileName);
                    string json = File.ReadAllText(fileName);
                    Uri uri = new Uri(ServiceUri, "/indexes/" + indexName + "/docs/index");
                    HttpResponseMessage response = AzureSearchHelper.SendSearchRequest(HttpClient, HttpMethod.Post, uri, json);
                    response.EnsureSuccessStatusCode();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", ex.Message);
            }
        }
    }
}
