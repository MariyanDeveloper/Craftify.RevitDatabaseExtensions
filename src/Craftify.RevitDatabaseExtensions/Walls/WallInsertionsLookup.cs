using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;

namespace Craftify.RevitDatabaseExtensions.Walls;

public static class WallInsertionsLookup
{
    public static IEnumerable<ElementId> FindInsertedIds(this Wall wall, Action<FindInsertOptions>? configOptions = default)
    {
        var options = new FindInsertOptions();
        configOptions?.Invoke(options);
        return wall.FindInserts(
            addRectOpenings: options.IncludeRectangularOpenings,
            includeShadows: options.IncludeShadows,
            includeEmbeddedWalls: options.IncludeEmbeddedWalls,
            includeSharedEmbeddedInserts: options.IncludeSharedEmbeddedInserts
        );
    }
    public static IEnumerable<Element> FindInsertedElements(this Wall wall, Action<FindInsertOptions>? configOptions = default)
    {
        return wall.FindInsertedIds()
            .Select(x => ElementFluentConversions.ToElement(x, wall.Document));
    }
}