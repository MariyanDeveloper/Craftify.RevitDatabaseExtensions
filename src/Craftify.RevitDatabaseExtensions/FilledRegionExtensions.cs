using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using Craftify.Geometry.BoundingBoxes;
using Craftify.Geometry.Enums;
using Craftify.Geometry.Extensions;
using Craftify.Geometry.Extensions.Points;

namespace Craftify.RevitDatabaseExtensions;

public static class FilledRegionExtensions
{
    public static Solid CreateExtrusion(this FilledRegion filledRegion, double extrusionValue)
    {
        return GeometryCreationUtilities
            .CreateExtrusionGeometry(
                filledRegion.GetBoundaries(),
                XYZ.BasisZ,
                extrusionValue);
    }

    public static Geometry.Collections.Profile GetProfile(this FilledRegion filledRegion) =>
        filledRegion
            .GetBoundaries()
            .ToProfile();

    public static XYZ GetCenter(this FilledRegion filledRegion)
    {
        var box = filledRegion.GetGeometryObjects<Solid>()
            .FirstOrDefault()
            ?.GetBoundingBox();
        if (box is not null)
        {
            return box.GetCenter(ApplyTransform.Yes);
        }

        var extrusion = 1; 
        return filledRegion
            .CreateExtrusion(extrusion)
            .ComputeCentroid()
            .MoveAlongVector(XYZ.BasisZ.Negate(), extrusion / 2 );
    }

    public static Geometry.Collections.Profile GetProfile(this IEnumerable<FilledRegion> filledRegions)
    {
        return filledRegions
            .SelectMany(x => x.GetBoundaries())
            .ToProfile();
    }
}