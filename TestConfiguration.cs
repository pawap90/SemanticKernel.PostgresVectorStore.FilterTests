using System.Runtime.CompilerServices;
using Microsoft.Extensions.Configuration;

namespace SemanticKernel.PostgresVectorStore.FilterTests;

public class TestConfiguration
{
    private readonly IConfigurationRoot _configRoot;
    private static TestConfiguration? s_instance;

    public static Postgres PostgresConfig => LoadSection<Postgres>();

    private TestConfiguration(IConfigurationRoot configRoot)
    {
        this._configRoot = configRoot;
    }

    public static TestConfiguration Initialize(IConfigurationRoot root)
    {
        s_instance = new TestConfiguration(root);
        return s_instance;
    }

    private static T LoadSection<T>([CallerMemberName] string? caller = null)
    {
        if (s_instance is null)
        {
            throw new InvalidOperationException(
                "TestConfiguration must be initialized with a call to Initialize(IConfigurationRoot) before accessing configuration values.");
        }

        if (string.IsNullOrEmpty(caller))
        {
            throw new ArgumentNullException(nameof(caller));
        }

        return s_instance._configRoot.GetSection(caller).Get<T>() ??
               throw new InvalidOperationException($"Configuration section '{caller}' not found.");
    }

    public class Postgres
    {
        public required string ConnectionString { get; set; }
    }
}