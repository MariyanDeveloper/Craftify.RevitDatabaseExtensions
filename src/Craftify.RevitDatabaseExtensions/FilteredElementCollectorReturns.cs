using Autodesk.Revit.DB;

namespace Craftify.RevitDatabaseExtensions;

public static class FilteredElementCollectorReturns
{
    public static FilteredElementCollector ToCollector(this Document document) =>
        new FilteredElementCollector(document);
    
    public static FilteredElementCollector ToCollector(this Document document, ElementId viewId) =>
        new FilteredElementCollector(document, viewId);
    
    public static FilteredElementCollector ToCollector(this Document document, View view) =>
        new FilteredElementCollector(document, view.Id);
}