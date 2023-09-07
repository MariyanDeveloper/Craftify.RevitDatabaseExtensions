namespace Craftify.RevitDatabaseExtensions;

public class WallJoinEndPointsResult
{
    public WallJoinResult Beginning { get; }
    public WallJoinResult End { get; }
    public bool IsAnyDisallowed => Beginning == WallJoinResult.Disallowed || End == WallJoinResult.Disallowed;

    public WallJoinEndPointsResult(WallJoinResult beginning, WallJoinResult end)
    {
        Beginning = beginning;
        End = end;
    }
}