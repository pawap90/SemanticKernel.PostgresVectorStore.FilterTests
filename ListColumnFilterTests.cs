using Microsoft.SemanticKernel;

namespace SemanticKernel.PostgresVectorStore.FilterTests;

public class ListColumnFilterTests : BaseTest
{
    [Fact]
    public async Task Filter_By_Single_Tag()
    {
        await UpsertSampleDataAsync(collection);

        List<VectorStoreRecord> results = [];
        await foreach (var result in collection.GetAsync(
            filter: r => r.Tags.Contains("pdf"),
            top: 10
        ))
        {
            results.Add(result);
        }

        Assert.Equal(2, results.Count);
        Assert.Contains(results, r => r.Id == 1);
        Assert.Contains(results, r => r.Id == 2);
    }

    [Fact]
    public async Task Filter_By_Multiple_Tags_As_Literal_String()
    {
        await UpsertSampleDataAsync(collection);

        List<VectorStoreRecord> results = [];
        await foreach (var result in collection.GetAsync(
            filter: r => r.Tags.Contains("pdf") || r.Tags.Contains("introduction") || r.Tags.Contains("faq"),
            top: 10
        ))
        {
            results.Add(result);
        }

        Assert.Equal(3, results.Count);
        Assert.Contains(results, r => r.Id == 1);
        Assert.Contains(results, r => r.Id == 2);
        Assert.Contains(results, r => r.Id == 4);
    }

    [Fact]
    public async Task Filter_By_Tags_Array_With_Any()
    {
        await UpsertSampleDataAsync(collection);

        string[] tagsToFilter = ["pdf", "introduction", "faq"];
        List<VectorStoreRecord> results = [];
        await foreach (var result in collection.GetAsync(
            filter: r => r.Tags.Any(tag => tagsToFilter.Contains(tag)),
            top: 10
        ))
        {
            results.Add(result);
        }

        Assert.Equal(3, results.Count);
        Assert.Contains(results, r => r.Id == 1);
        Assert.Contains(results, r => r.Id == 2);
        Assert.Contains(results, r => r.Id == 4);
    }
}