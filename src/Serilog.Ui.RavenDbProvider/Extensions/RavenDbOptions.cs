using Ardalis.GuardClauses;
using Raven.Client.Documents;
using Serilog.Ui.Core.OptionsBuilder;

namespace Serilog.Ui.RavenDbProvider.Extensions;

/// <inheritdoc />
public class RavenDbOptions : IDbOptions
{
    /// <summary>
    /// Required parameter.
    /// </summary>
    public IDocumentStore DocumentStore { get; private set; } = new DocumentStore();

    /// <summary>
    /// Optional parameter. Defaults to LogEvents.
    /// </summary>
    public string CollectionName { get; private set; } = "LogEvents";

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
        Guard.Against.Null(DocumentStore, nameof(DocumentStore));
        Guard.Against.NullOrEmpty(DocumentStore.Urls, nameof(DocumentStore.Urls));
        Guard.Against.NullOrWhiteSpace(DocumentStore.Database, nameof(DocumentStore.Database));
        Guard.Against.NullOrWhiteSpace(CollectionName, nameof(CollectionName));
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
}