using System;
using Autodesk.Revit.DB;
using Craftify.Geometry.Extensions.Curves;
using Craftify.Geometry.Extensions.Solids;

namespace Craftify.RevitDatabaseExtensions;

public static class WallJoinExtensions
{
    public static bool IsBeginningJoinAllowed(this Wall wall)
    {
        return WallUtils.IsWallJoinAllowedAtEnd(wall, 0);
    }
    
    public static bool IsEndJoinAllowed(this Wall wall)
    {
        return WallUtils.IsWallJoinAllowedAtEnd(wall, 1);
    }

    public static bool IsAnyJoinAllowed(this Wall wall)
    {
        return wall.IsEndJoinAllowed() || wall.IsBeginningJoinAllowed();
    }
    public static bool IsAnyJoinDisallowed(this Wall wall)
    {
        return wall.IsEndJoinAllowed() is false || wall.IsBeginningJoinAllowed() is false;
    }

    public static void AllowJoinsIfDisallowed(this Wall wall)
    {
        var wallJoinEndPointsResult = wall.CalculateJoinEndPoints();
        if (wallJoinEndPointsResult.Beginning == WallJoinResult.Disallowed)
        {
            WallUtils.AllowWallJoinAtEnd(wall, 0);
        }

        if (wallJoinEndPointsResult.End == WallJoinResult.Disallowed)
        {
            WallUtils.AllowWallJoinAtEnd(wall, 1);
        }
    }

    public static WallJoinEndPointsResult CalculateJoinEndPoints(this Wall wall)
    {
        var beginningWallJoinResult = wall.IsBeginningJoinAllowed()
            ? WallJoinResult.Allowed
            : WallJoinResult.Disallowed;
        var endWallJoinResult = wall.IsEndJoinAllowed()
            ? WallJoinResult.Allowed
            : WallJoinResult.Disallowed;
        return new WallJoinEndPointsResult(beginningWallJoinResult, endWallJoinResult);
    }
    public static bool IsConnectedTo(this Wall fromWall, Wall toWall, Action<WallConnectionOptions>? configOptions = null)
    {
        var options = new WallConnectionOptions();
        configOptions?.Invoke(options);
        if (options.IncludeAnalysisForDisallowedJoins is false)
        {
            return fromWall.IsConnectedToUsingCurveIntersectionComparison(toWall);
        }
        if (HasAnyWallDisallowedJoins(fromWall, toWall))
        {
            return fromWall.IsConnectedToUsingSolidComparison(toWall);
        }
        return fromWall.IsConnectedToUsingCurveIntersectionComparison(toWall);

    }
    
    private static bool IsConnectedToUsingCurveIntersectionComparison(this Wall fromWall, Wall toWall)
    {
        return fromWall.GetPlacementCurve().IntersectsWith(toWall.GetPlacementCurve());
    }
    private static bool IsConnectedToUsingSolidComparison(this Wall fromWall, Wall toWall)
    {
        var fromWallSolid = fromWall.GetGeometryObjects<Solid>().Union();
        var toWallSolid = toWall.GetGeometryObjects<Solid>().Union();
        var solidRelationship = fromWallSolid.CalculateRelationshipWith(toWallSolid);
        return solidRelationship == SolidRelationship.Touching;
    }
    private static bool HasAnyWallDisallowedJoins(Wall fromWall, Wall toWall)
    {
        return fromWall.IsAnyJoinDisallowed() || toWall.IsAnyJoinDisallowed();
    }
}