using System.Globalization;

namespace FIRE;

public enum FIRECatalogStage
{
    Collect,
    Generate,
    Execute,
    Inspect
}

public enum FIRECatalogMessageLevel
{
    Trace,
    Info,
    Warning,
    Error
}

public sealed class FIRECatalogProgressEventArgs : EventArgs
{
    public required FIRECatalogStage Stage { get; init; }
    public required FIRECatalogMessageLevel Level { get; init; }
    public required string Message { get; init; }
    public string? MessageKey { get; init; }
    public string? CurrentFilePath { get; init; }
    public int ProcessedCount { get; init; }
    public int TotalCount { get; init; }
    public CultureInfo? Culture { get; init; }
}
