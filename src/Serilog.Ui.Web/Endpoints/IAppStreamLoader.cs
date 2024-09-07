namespace Serilog.Ui.Web.Endpoints;

/// <summary>
/// Provides methods to load application streams.
/// </summary>
internal interface IAppStreamLoader
{
    /// <summary>
    /// Gets the index stream.
    /// </summary>
    /// <returns>The index stream, or null if the stream is not available.</returns>
    Stream? GetIndex();
}