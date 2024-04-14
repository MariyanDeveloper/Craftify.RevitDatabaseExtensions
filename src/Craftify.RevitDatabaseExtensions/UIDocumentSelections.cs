using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Craftify.RevitDatabaseExtensions;

public static class UIDocumentSelections
{
    public static IEnumerable<Element> GetSelectedElements(this UIDocument uiDocument)
    {
        return uiDocument.Selection
            .GetElementIds()
            .Select(id => uiDocument.Document.GetElement(id));
    }

    public static T GetSingleSelectedElement<T>(this UIDocument uiDocument)
        where T: Element
    {
        return uiDocument.Selection
            .GetElementIds()
            .Single()
            .ToElement<T>(uiDocument.Document);
    }

}