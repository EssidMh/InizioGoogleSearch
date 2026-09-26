using System.Text.Json;

namespace InizioGoogleSearch.Tests;

public class UnitTest1
{
    [Fact]
    public async Task SaveResultsAsync_ShouldCreateJsonFile()
    {
        var results = new List<SearchResult>
        {
            new SearchResult
            {
                Position = 1,
                Title = "Test Restaurant",
                Link = "https://example.com",
                Snippet = "Test snippet"
            }
        };

        var service = new SearchService();

        await service.SaveResultsAsync(results);

        Assert.True(File.Exists("results.json"));

        var json = await File.ReadAllTextAsync("results.json");

        var savedResults =
            JsonSerializer.Deserialize<List<SearchResult>>(json);

        Assert.NotNull(savedResults);
        Assert.Single(savedResults);

        Assert.Equal("Test Restaurant", savedResults[0].Title);
        Assert.Equal("https://example.com", savedResults[0].Link);
        Assert.Equal(1, savedResults[0].Position);
        Assert.Equal("Test snippet", savedResults[0].Snippet);
    }

    [Fact]
    public async Task SaveResultsAsync_ShouldSaveMultipleResults()
    {
        var results = new List<SearchResult>
        {
            new SearchResult
            {
                Position = 1,
                Title = "Restaurant One",
                Link = "https://example.com/one",
                Snippet = "First restaurant"
            },
            new SearchResult
            {
                Position = 2,
                Title = "Restaurant Two",
                Link = "https://example.com/two",
                Snippet = "Second restaurant"
            },
            new SearchResult
            {
                Position = 3,
                Title = "Restaurant Three",
                Link = "https://example.com/three",
                Snippet = "Third restaurant"
            }
        };

        var service = new SearchService();

        await service.SaveResultsAsync(results);

        var json = await File.ReadAllTextAsync("results.json");

        var savedResults =
            JsonSerializer.Deserialize<List<SearchResult>>(json);

        Assert.NotNull(savedResults);
        Assert.Equal(3, savedResults.Count);
    }
}