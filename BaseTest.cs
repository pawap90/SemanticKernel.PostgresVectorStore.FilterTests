using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.VectorData;
using Microsoft.SemanticKernel;

namespace SemanticKernel.PostgresVectorStore.FilterTests;

public abstract class BaseTest
{
    protected readonly VectorStore vectorStore;
    protected readonly VectorStoreCollection<int, VectorStoreRecord> collection;

    protected BaseTest()
    {
        IConfigurationRoot configRoot = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile("appsettings.Development.json", true)
            .Build();

        TestConfiguration.Initialize(configRoot);

        var kernel = CreateKernel();
        this.vectorStore = kernel.GetRequiredService<VectorStore>();
        this.collection = vectorStore.GetCollection<int, VectorStoreRecord>("vector_store_records");
    }

    private static Kernel CreateKernel()
    {
        var kernelBuilder = Kernel.CreateBuilder();

        kernelBuilder.Services
            .AddPostgresVectorStore(TestConfiguration.PostgresConfig.ConnectionString)
            .AddVectorStoreTextSearch<VectorStoreRecord>();

        return kernelBuilder.Build();
    }

    protected class VectorStoreRecord
    {
        [VectorStoreKey]
        public required int Id { get; set; }

        [VectorStoreData]
        public required string OriginFilename { get; set; }

        [VectorStoreData(StorageName = "tags")]
        public List<string> Tags { get; set; } = [];

        [VectorStoreData]
        public required string Text { get; set; }
    }

    protected readonly List<VectorStoreRecord> sampleRecords = [
        new VectorStoreRecord
        {
            Id = 1,
            OriginFilename = "user-manual.pdf",
            Tags = [ "manual", "pdf", "introduction" ],
            Text = "Chapter 1: Introduction\nIn this manual, we will cover the basics of using our product...",
        },
        new VectorStoreRecord
        {
            Id = 2,
            OriginFilename = "user-manual.pdf",
            Tags = [ "manual", "pdf", "installation" ],
            Text = "Chapter 2: Installation\nTo install the product, follow these steps...",
        },
        new VectorStoreRecord
        {
            Id = 3,
            OriginFilename = "troubleshooting-guide.md",
            Tags = [ "guide", "md", "troubleshooting" ],
            Text = "Troubleshooting Common Issues\nIf you encounter problems, try the following solutions...",
        },
        new VectorStoreRecord
        {
            Id = 4,
            OriginFilename = "troubleshooting-guide.md",
            Tags = [ "guide", "md", "faq" ],
            Text = "Frequently Asked Questions\nHere are some common questions and answers about our product...",
        }
    ];

    protected async Task UpsertSampleDataAsync(VectorStoreCollection<int, VectorStoreRecord> collection)
    {
        await collection.EnsureCollectionExistsAsync();
        await collection.UpsertAsync(sampleRecords);
    }
}
