using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using Craftify.Geometry.BoundingBoxes;
using Craftify.Geometry.Enums;

namespace Craftify.RevitDatabaseExtensions.WallJoins;

public static class WallExtensions
{
    public static double GetHeight(this Wall wall)
    {
        return wall.get_BoundingBox(default).CalculateSideDimension(Side.Height);
    }
    public static bool MatchesOrientation(this Wall wall, XYZ vector) => wall.Orientation.IsAlmostEqualTo(vector);
    public static bool MatchesOrientation(this Wall wall, Wall otherWall) => wall.Orientation.IsAlmostEqualTo(otherWall.Orientation);
    
    public static ElementId GetTopConstraintLevelId(this Wall wall) =>
        wall.GetParameterByBuiltInParameter(BuiltInParameter.WALL_HEIGHT_TYPE)
            .AsElementId();

    public static Level GetBaseConstraintLevel(this Wall wall) =>
        wall.GetConstraintLevel(BuiltInParameter.WALL_BASE_CONSTRAINT);

    public static Level GetTopConstraintLevel(this Wall wall) =>
        wall.GetConstraintLevel(BuiltInParameter.WALL_HEIGHT_TYPE);
    
    private static Level GetConstraintLevel(this Wall wall, BuiltInParameter builtInParameter) =>
        wall.GetParameterByBuiltInParameter(builtInParameter)
            .AsElementId()
            .ToElement<Level>(wall.Document);
    
    public static Level GetConstraintLevel(this Wall wall, WallLevelConstraint levelConstraint)
    {
        return levelConstraint switch
        {
            WallLevelConstraint.Top => wall.GetTopConstraintLevel(),
            WallLevelConstraint.Base => wall.GetBaseConstraintLevel(),
            _ => throw new ArgumentOutOfRangeException(nameof(levelConstraint), levelConstraint, null)
        };
    }

    public static (Level BaseConstraint, Level TopConstraint) GetConstraintLevels(this Wall wall)
    {
        return (
            wall.GetBaseConstraintLevel(),
            wall.GetTopConstraintLevel()
        );
    }
    
    public static bool IsUnconnected(this Wall wall) =>
        wall.GetParameterByBuiltInParameter(BuiltInParameter.WALL_HEIGHT_TYPE)
            .AsElementId() == ElementId.InvalidElementId;
    
    public static IEnumerable<Wall> WhereOrientationMatches(this IEnumerable<Wall> walls, XYZ orientation)
    {
        return walls
            .Where(w => w.MatchesOrientation(orientation));
    }
    public static IEnumerable<Wall> WhereOrientationMatches(this IEnumerable<Wall> walls, Wall wall)
    {
        return walls
            .Where(w => w.MatchesOrientation(wall));
    }
}