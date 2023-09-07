using System.Collections.Generic;
using Autodesk.Revit.DB;
using Craftify.RevitDatabaseExtensions.Collections;

namespace Craftify.RevitDatabaseExtensions;

public static class ElementsExtension
{
    public static GroupChildren<T> ToGroupChildren<T>(this IEnumerable<T> elements) where T: Element
    {
        return new (elements);
    }
}