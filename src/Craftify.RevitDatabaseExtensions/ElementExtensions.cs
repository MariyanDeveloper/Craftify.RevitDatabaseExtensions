using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using Craftify.RevitDatabaseExtensions.Exceptions;


namespace Craftify.RevitDatabaseExtensions;

public static class ElementExtensions
{
    public static Element GetElementType(this Element element)
    {
        ElementArgumentNullException.ThrowIfElementNull(element);
        return element.Document.GetElement(element.GetTypeId());
    }
    public static TElement Clone<TElement>(this TElement element)
        where TElement : Element
    {
        ElementArgumentNullException.ThrowIfElementNull(element);
        var document = element.Document;
        return (TElement)document.GetElement(ElementTransformUtils
            .CopyElement(
                document,
                element.Id,
                XYZ.Zero).First());
    }
    
    
    public static void ChangeCurveLocation(this Element element, Curve curve)
    {
        if (curve is null) throw new ArgumentNullException(nameof(curve));
        ElementArgumentNullException.ThrowIfElementNull(element);
        ElementArgumentNullException.ThrowIfElementNotCurveDriven(element);
        ((LocationCurve)element.Location).Curve = curve;
    }


    public static void ChangePointLocation(this Element element, XYZ point)
    {
        if (point is null) throw new ArgumentNullException(nameof(point));
        ElementArgumentNullException.ThrowIfElementNull(element);
        ElementArgumentNullException.ThrowIfElementNotPointDriven(element);
        ((LocationPoint)element.Location).Point = point;
    }


    public static void Move(this Element element, XYZ translation)
    {
        if (translation == null) throw new ArgumentNullException(nameof(translation));
        ElementArgumentNullException.ThrowIfElementNull(element);
        var document = element.Document;
        ElementTransformUtils.MoveElement(
            document,
            element.Id,
            translation);
    }

    public static void Move(this IEnumerable<Element> elements, XYZ translation)
    {
        if (elements is null) throw new ArgumentNullException(nameof(elements));
        if (translation is null) throw new ArgumentNullException(nameof(translation));
        if (elements.Any() is false)
        {
            return;
        }
        var document = elements.First().Document;
        var ids = elements.Select(e => e.Id).ToList();
        ElementTransformUtils.MoveElements(
            document,
            ids,
            translation);
    }
    public static IEnumerable<T> GetCutGeometryObjectsOnView<T>(
        this Element element,
        View view,
        Options? options = null) where T : GeometryObject
    {
        options ??= new Options();
        options.View = view;
        return element
            .GetGeometryObjects<T>(options);
    }
    
    public static bool HasGeometryElement(this Element element) => element.get_Geometry(new Options()) is not null;

}