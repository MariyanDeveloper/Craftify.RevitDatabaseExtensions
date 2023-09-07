using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;

namespace Craftify.RevitDatabaseExtensions;

public static class GroupExtensions
{
    public static List<Element> GetChildren(this Group group)
    {
        return group
            .GetMemberIds()
            .Select(x => group.Document.GetElement((ElementId)x))
            .ToList();
    }

    public static List<T> GetChildren<T>(this Group group) where T : Element
    {
        return group
            .GetMemberIds()
            .Select(x => group.Document.GetElement(x))
            .OfType<T>()
            .ToList();
    }
}