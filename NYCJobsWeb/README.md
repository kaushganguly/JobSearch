# NYC Jobs web application

The application targets .NET 10 and runs as an ASP.NET Core MVC web app:

```sh
dotnet run --project NYCJobsWeb.csproj
```

Configure Azure Cognitive Search with `Search__Endpoint` (or
`Search__ServiceName`) and `Search__ApiKey` environment variables. Set
`BingMaps__ApiKey` to enable the map. These settings can also be supplied in
`appsettings.json`; do not commit service keys.
