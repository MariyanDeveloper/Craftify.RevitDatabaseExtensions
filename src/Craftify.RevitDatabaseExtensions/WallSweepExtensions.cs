using Autodesk.Revit.DB;

namespace Craftify.RevitDatabaseExtensions;

public static class WallSweepExtensions
{
    public static bool IsRevealVertical(this WallSweep wallSweep)
    {
        var wallSweepInfo = wallSweep.GetWallSweepInfo();
        return wallSweepInfo.WallSweepType == WallSweepType.Reveal && wallSweepInfo.IsVertical;
    }
}