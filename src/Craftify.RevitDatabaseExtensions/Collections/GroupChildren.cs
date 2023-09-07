using System.Collections.Generic;
using Autodesk.Revit.DB;

namespace Craftify.RevitDatabaseExtensions.Collections;

public class GroupChildren : List<Element>
{
    public GroupChildren() { }
    public GroupChildren(IEnumerable<Element> elements): base(elements) { }
}