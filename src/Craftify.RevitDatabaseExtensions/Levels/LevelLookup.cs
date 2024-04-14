using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using Craftify.Functional;
using Craftify.Geometry.Extensions.Points;

namespace Craftify.RevitDatabaseExtensions.Levels;


public static class LevelCalculations
{
    public static double CalculateHeightToLevel(
        this Level fromLevel, Level toLevel)
    {
        var fromOrigin = fromLevel.GetOriginAtProjectElevation();
        var toOrigin = toLevel.GetOriginAtProjectElevation();
        return fromOrigin
            .MeasureSignedDistance(toOrigin, XYZ.BasisZ);
    }
}

public static class LevelSelections
{
    public static XYZ GetOriginAtProjectElevation(this Level level)
    {
        var levelElevation = level.ProjectElevation;
        return new XYZ(0, 0, levelElevation);
    }
    
    public static Plane GetPlaneAtProjectElevation(this Level level)
    {
        var plane = Plane.CreateByNormalAndOrigin(
            XYZ.BasisZ,
            level.GetOriginAtProjectElevation());
        return plane;
    }
}

public static class LevelLookup
{
    public static Option<Level> FindNearestLevelAbove(this Level level)
    {
        return level
            .FindLevelsWherePasses(l => l.Elevation > level.Elevation)
            .OrderBy(l => l.Elevation)
            .FirstOrDefault();
    }
    
    public static Option<Level> FindNearestLevelBelow(this Level level)
    {
        return level
            .FindLevelsWherePasses(l => l.Elevation < level.Elevation)
            .OrderByDescending(l => l.Elevation)
            .FirstOrDefault();
    }
    
    private static IEnumerable<Level> FindLevelsWherePasses(this Level level, Func<Level, bool> whereClause)
    {
        return level
            .Document
            .ToCollector()
            .OfClass<Level>()
            .Excluding(level.Id)
            .ToElements<Level>()
            .Where(whereClause);
    }
}