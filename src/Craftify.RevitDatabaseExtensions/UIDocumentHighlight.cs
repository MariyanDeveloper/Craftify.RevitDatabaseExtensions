using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Craftify.RevitDatabaseExtensions;

public static class UIDocumentHighlight
{
    public static void Highlight(this UIDocument uiDocument, Element element)
    {
        uiDocument.Selection.SetElementIds(new List<ElementId>() { element.Id });
    }
        
    public static void Highlight(this UIDocument uiDocument, ICollection<ElementId> elementIds)
    {
        uiDocument.Selection.SetElementIds(elementIds);
    }

    public static void Highlight(this UIDocument uiDocument, IEnumerable<Element> elements)
    {
        uiDocument.Selection
            .SetElementIds(
                elements.Select(e => e.Id).ToList()
            );
    }
    public static void Highlight(this UIDocument uiDocument, params Element[] elements)
    {
        uiDocument.Selection
            .SetElementIds(elements.Select(e => e.Id).ToList()
            );
    }
}