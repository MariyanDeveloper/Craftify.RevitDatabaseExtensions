using Autodesk.Revit.DB;
using Craftify.Geometry.Extensions.Points;
using Document = Autodesk.Revit.Creation.Document;


namespace Craftify.RevitDatabaseExtensions;

public static class DocumentCreationExtensions
{
    public static ReferencePlane CreateReferencePlane(
        this Document document,
        Plane plane,
        View view,
        string name,
        double extension = 10)
    {
        var origin = plane.Origin;
        var bubbleEnd = origin.MoveAlongVector(plane.XVec * extension);
        var freeEnd = origin.MoveAlongVector(plane.XVec * -extension);
        var referencePlane = document.NewReferencePlane(bubbleEnd, freeEnd, plane.YVec, view);
        referencePlane.Name = name;
        return referencePlane;
    }
}