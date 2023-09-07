using System.Collections.Generic;
using Autodesk.Revit.DB;

namespace Craftify.RevitDatabaseExtensions.Collections;

public class GroupChildren<T> : List<T> where T: Element
{
    public GroupChildren() { }
    public GroupChildren(IEnumerable<T> elements): base(elements) { }
}