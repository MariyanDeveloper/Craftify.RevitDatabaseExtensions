using System.Collections.Generic;
using Autodesk.Revit.DB;
using Craftify.RevitDatabaseExtensions.Collections;

namespace Craftify.RevitDatabaseExtensions;

public static class ElementsExtension
{
    public static GroupChildren ToGroupChildren(this IEnumerable<Element> elements) => new (elements);
}