using System;
using System.Collections.Generic;
using Autodesk.Revit.DB;
using Craftify.Geometry.BoundingBoxes;
using Craftify.Geometry.Enums;
using Craftify.Geometry.Extensions.Solids;

namespace Craftify.RevitDatabaseExtensions.IntersectionLookup;

public static class IntersectionLookupExtensions
{
    public static IEnumerable<Element> FindElementsIntersectingWithBox(
        this Document document,
        BoundingBoxXYZ boundingBoxXYZ,
        Action<BoundingBoxNeighborhoodLookupOptions>? configOptions = default)
    {
        var options = new BoundingBoxNeighborhoodLookupOptions();
        configOptions?.Invoke(options);
        var boxSolid = boundingBoxXYZ.ConvertToSolid(
            configOptions: cuboidOptions => { cuboidOptions.ApplyTransform = ApplyTransform.Yes; });
        var intersectionBox = boxSolid.GetBoundingBox();
        var boundingBoxIntersectionFilter = intersectionBox.ConvertToIntersectionFilter();
        var collector = (options.ViewId == ElementId.InvalidElementId)
            ? document.ToCollector()
            : document.ToCollector(options.ViewId);
        
        if (options.PreFilter is not null)
        {
            collector = collector.WherePasses(options.PreFilter);
        }
        collector = collector
            .WherePasses(boundingBoxIntersectionFilter);
        if (options.PerformPostSolidCheck)
        {
            var solidIntersectionFilter = boxSolid.ConvertToIntersectionFilter();
            collector = collector
                .WherePasses(solidIntersectionFilter);
        }
        if (options.PostFilter is not null)
        {
            collector = collector.WherePasses(options.PostFilter);
        }

        return collector.ToElements();

    }
}