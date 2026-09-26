using System.Text.Json;

public class SearchService
{
    public async Task<List<SearchResult>> SaveResultsAsync(
        List<SearchResult> results)
    {
        var jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        var outputJson = JsonSerializer.Serialize(results, jsonOptions);

        await File.WriteAllTextAsync("results.json", outputJson);

        return results;
    }
}