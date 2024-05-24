using Ardalis.GuardClauses;
using Raven.Client.Documents;
using Serilog.Ui.Core.OptionsBuilder;

namespace Serilog.Ui.RavenDbProvider.Extensions;

/// <inheritdoc />
public class RavenDbOptions : IDbOptions
{
    private string _customProviderName = string.Empty;

    /// <summary>
    /// Required parameter.
    /// </summary>
    public IDocumentStore DocumentStore { get; private set; } = new DocumentStore();

    /// <summary>
    /// Optional parameter. Defaults to LogEvents.
    /// </summary>
    public string CollectionName { get; private set; } = "LogEvents";

    /// <summary>
    /// Provider name.
    /// </summary>
    public string ProviderName
        => !string.IsNullOrWhiteSpace(_customProviderName) ? _customProviderName : string.Join(".", "RavenDB", CollectionName);

    /// <summary>
    /// Throws if DocumentStore is null.
    /// Throws if DocumentStore.Urls is null or empty.
    /// Throws if DocumentStore.Urls is null or empty.
    /// Throws if CollectionName is null or whitespace.
    /// Throws if DatabaseName is null or whitespace and is not found in the connection string.
    /// </summary>
    /// <exception cref="ArgumentException"></exception>
    public void Validate()
    {
        Guard.Against.Null(DocumentStore);
        Guard.Against.NullOrEmpty(DocumentStore.Urls);
        Guard.Against.NullOrWhiteSpace(DocumentStore.Database);
        Guard.Against.NullOrWhiteSpace(CollectionName);
    }

    /// <summary>
    /// Fluently sets DocumentStore.
    /// </summary>
    /// <param name="documentStore"></param>
    public RavenDbOptions WithDocumentStore(IDocumentStore documentStore)
    {
        DocumentStore = documentStore;
        return this;
    }

    /// <summary>
    /// Fluently sets CollectionName.
    /// </summary>
    /// <param name="collectionName"></param>
    public RavenDbOptions WithCollectionName(string collectionName)
    {
        CollectionName = collectionName;
        return this;
    }

    /// <summary>
    /// Fluently sets CustomProviderName.
    /// </summary>
    /// <param name="customProviderName"></param>
    public RavenDbOptions WithCustomProviderName(string customProviderName)
    {
        _customProviderName = customProviderName;
        return this;
    }
}