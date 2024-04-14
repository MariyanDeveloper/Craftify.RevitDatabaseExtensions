using Autodesk.Revit.DB;

namespace Craftify.RevitDatabaseExtensions;

public static class ElementFluentConversions
{
    public static ElementId ToElementId(this int value) => new ElementId(value);
    public static T ToElement<T>(this ElementId id, Document document)
        where T : Element
    {
        return document.GetElement<T>(id);
    }
    public static Element ToElement(this ElementId id, Document document)
    {
        return document.GetElement(id);
    }

    public static T Cast<T>(this Element element)
        where T : Element => (T)element;
}

public static class ElementSelections
{
    public static T GetHost<T>(
        this FamilyInstance familyInstance)
        where T: Element
    {
        return familyInstance.Host.Cast<T>();
    }
}