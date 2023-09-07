using System.Linq;
using Autodesk.Revit.DB;
using Craftify.RevitDatabaseExtensions.Collections;

namespace Craftify.RevitDatabaseExtensions;

public static class GroupExtensions
{
    public static GroupChildren GetChildren(this Group group)
    {
        return group
            .GetMemberIds()
            .Select(x => group.Document.GetElement((ElementId)x))
            .ToGroupChildren();
    }

    public static GroupChildren GetChildren<T>(this Group group) where T : Element
    {
        return group
            .GetMemberIds()
            .Select(x => group.Document.GetElement(x))
            .OfType<T>()
            .ToGroupChildren();
    }
}