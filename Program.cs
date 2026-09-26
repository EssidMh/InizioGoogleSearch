using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddHttpClient();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseHttpsRedirection();

app.MapGet("/api/search", async (
    string q,
    IConfiguration config,
    HttpClient httpClient) =>
{
    var apiKey = config["SerpApi:ApiKey"];

    if (string.IsNullOrWhiteSpace(apiKey))
    {
        return Results.Problem("SerpApi API key is not configured.");
    }

    var url =
        $"https://serpapi.com/search.json?engine=google&q={Uri.EscapeDataString(q)}&api_key={apiKey}";

    var response = await httpClient.GetAsync(url);

    if (!response.IsSuccessStatusCode)
    {
        return Results.Problem(
            $"SerpApi request failed with status code {(int)response.StatusCode}.");
    }

    var json = await response.Content.ReadAsStringAsync();

    var data = JsonSerializer.Deserialize<SerpApiResponse>(json);

    var results = data?.OrganicResults ?? new List<SearchResult>();

    var searchService = new SearchService();

    await searchService.SaveResultsAsync(results);

    return Results.Ok(results);
});

app.Run();