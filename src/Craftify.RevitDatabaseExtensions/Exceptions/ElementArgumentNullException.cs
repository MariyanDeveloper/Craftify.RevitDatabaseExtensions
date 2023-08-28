using Autodesk.Revit.DB;

namespace Craftify.RevitDatabaseExtensions.Exceptions;

public static class ElementArgumentNullException
{
    public static void ThrowIfElementNotCurveDriven(Element element)
    {
        if (element.Location is not LocationCurve)
        {
            throw new System.ArgumentNullException($"{element.Name} is not curve-driven");
        }
    }
    public static void ThrowIfElementNotPointDriven(Element element)
    {
        if (element.Location is not LocationPoint)
        {
            throw new System.ArgumentNullException($"{element.Name} is not point-driven");
        }
    }

    public static void ThrowIfElementNull(Element element)
    {
        if (element is null)
        {
            throw new System.ArgumentNullException(nameof(element));
        }
    }
}