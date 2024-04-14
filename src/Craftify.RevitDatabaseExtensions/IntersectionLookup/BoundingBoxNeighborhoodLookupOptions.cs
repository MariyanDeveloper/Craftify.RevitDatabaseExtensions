using Autodesk.Revit.DB;

namespace Craftify.RevitDatabaseExtensions.IntersectionLookup;

public class BoundingBoxNeighborhoodLookupOptions
{
    public ElementFilter? PreFilter { get; set; } = default;
    public ElementFilter? PostFilter { get; set; } = default;
    public ElementId ViewId { get; set; } = ElementId.InvalidElementId;
    public bool PerformPostSolidCheck { get; set; } = true;
}