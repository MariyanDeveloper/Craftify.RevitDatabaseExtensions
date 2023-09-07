using Autodesk.Revit.DB;
using Craftify.Geometry.Extensions;

namespace Craftify.RevitDatabaseExtensions.Views;

public static class ViewPlanExtensions
{
    public static XYZ GetOriginOnElevation(this ViewPlan viewPlan)
    {
        var level = viewPlan.GenLevel;
        var elevation = level.Elevation;
        return XYZ.Zero.MoveAlongVector(XYZ.BasisZ * elevation);
    }
    public static Plane GetCutPlane(this ViewPlan viewPlan)
    {
        var cutPlaneOffset = viewPlan.GetViewRange().GetOffset(PlanViewPlane.CutPlane);
        var newOrigin = viewPlan.GetOriginOnElevation().MoveAlongVector(XYZ.BasisZ * cutPlaneOffset);
        return Plane.CreateByNormalAndOrigin(
            XYZ.BasisZ,
            newOrigin);
    }
}