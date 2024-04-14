using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;

namespace Craftify.RevitDatabaseExtensions;

public static class FilteredElementCollectorReturnsLookup
{
    public static FilteredElementCollector OfClass<T>(this FilteredElementCollector collector) where T : Element
    {
        return collector.OfClass(typeof(T));
    }
    public static FilteredElementCollector Excluding(this FilteredElementCollector collector, params ElementId[] idsToExclude)
    {
        return collector.Excluding(idsToExclude);
    }
    public static FilteredElementCollector WherePassesSequentially(
        this FilteredElementCollector collector,
        params ElementFilter[] filters)
    {
        return filters
            .Aggregate(collector, 
                (accumulation, currentFilter) => accumulation.WherePasses(currentFilter));
    }
    
    public static IEnumerable<T> ToElements<T>(this FilteredElementCollector collector) where T : Element
    {
        return  collector
            .ToElements()
            .Cast<T>()
            .ToArray();
    }
}