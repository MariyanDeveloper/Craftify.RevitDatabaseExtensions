using Autodesk.Revit.DB;
using Craftify.Geometry.BoundingBoxes;
using Craftify.Geometry.Enums;
using Craftify.Geometry.Extensions;
using Craftify.Geometry.Extensions.Curves;
using Craftify.Geometry.Extensions.Points;
using Craftify.Geometry.Extensions.Solids;
using Craftify.Geometry.GeometryExtraction;

namespace Craftify.RevitDatabaseExtensions.Walls;

public static class WallCalculations
{
    public static double GetHeight(this Wall wall)
    {
        return wall.get_BoundingBox(default).CalculateSideDimension(Side.Height);
    }
    public static Transform CalculateTransform(
        this Wall wall)
    {
        var basisFacingDirection = XYZ.BasisY;
        var (axis, angle) = basisFacingDirection.CalculateAlignmentResultTo(
            wall.Orientation);
        return Transform.CreateRotation(axis, -angle)
            .SetOrigin(wall.GetPlacementCurve().GetCenter());
    }
    

    public static BoundingBoxXYZ CalculateOrientedBoundingBox(
        this Wall wall,
        Options? geometryOptions = default)
    {
        geometryOptions ??= new Options();
        var wallTransform = wall.CalculateTransform();
        var(min,max) = wall.get_Geometry(geometryOptions)
            .SelectChildrenOfType<Solid>()
            .Union()
            .CreateTransformed(wallTransform.Inverse)
            .GetBoundingBox()
            .GetCornerVertices(ApplyTransform.Yes);
        return BoundingBox.Create(
            min,
            max,
            wallTransform
        );
    }

}