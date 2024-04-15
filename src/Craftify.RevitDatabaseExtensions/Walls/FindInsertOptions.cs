namespace Craftify.RevitDatabaseExtensions.Walls;

public class FindInsertOptions
{
    public bool IncludeRectangularOpenings { get; set; } = false;
    public bool IncludeShadows { get; set; } = false;
    public bool IncludeEmbeddedWalls { get; set; } = false;
    public bool IncludeSharedEmbeddedInserts { get; set; } = false;
}