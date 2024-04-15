using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;

namespace Craftify.RevitDatabaseExtensions.Walls;

public static class WallEqualities
{
    public static bool IsUnconnected(this Wall wall) =>
        wall.GetParameterByBuiltInParameter(BuiltInParameter.WALL_HEIGHT_TYPE)
            .AsElementId() == ElementId.InvalidElementId;
    
    public static IEnumerable<Wall> WhereOrientationMatches(this IEnumerable<Wall> walls, XYZ orientation)
    {
        return walls
            .Where(w => MatchesOrientation(w, orientation));
    }
    public static IEnumerable<Wall> WhereOrientationMatches(this IEnumerable<Wall> walls, Wall wall)
    {
        return walls
            .Where(w => w.MatchesOrientation(wall));
    }
    
    public static bool MatchesOrientation(this Wall wall, XYZ vector) => wall.Orientation.IsAlmostEqualTo(vector);
    public static bool MatchesOrientation(this Wall wall, Wall otherWall) => wall.Orientation.IsAlmostEqualTo(otherWall.Orientation);
}