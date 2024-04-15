using System;
using Autodesk.Revit.DB;

namespace Craftify.RevitDatabaseExtensions.Walls;

public static class WallConstraintsSelectors
{
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
    
   
}