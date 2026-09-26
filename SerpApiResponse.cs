using System.Text.Json.Serialization;

public class SerpApiResponse
{
    [JsonPropertyName("organic_results")]
    public List<SearchResult>? OrganicResults { get; set; }
}