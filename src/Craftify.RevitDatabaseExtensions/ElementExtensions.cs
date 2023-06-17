using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using Craftify.Geometry;
using Craftify.Geometry.Enums;

namespace Craftify.RevitDatabaseExtensions;

public static class ElementExtensions
{
    public static IEnumerable<Solid> GetSolids(this Element element, Options? geometryOptions = null)
    {
        geometryOptions ??= new Options()
        {
            DetailLevel = ViewDetailLevel.Fine
        };
        return element.GetGeometryObjects<Solid>(geometryOptions: geometryOptions)
            .Where(s => s.HasVolume());
    }

    public static Curve GetPlacementCurve(this Element element)
    {
        if (!(element.Location is LocationCurve locationCurve))
            throw new ArgumentNullException($"The {element.Name} is not curve-driven.");
        return locationCurve.Curve;
    }

    public static XYZ GetPlacementPoint(
        this Element element)
    {
        if (!(element.Location is LocationPoint locationPoint))
            throw new ArgumentNullException($"The {element.Name} is not point-driven.");
        return locationPoint.Point;
    }

    public static Element GetElementType(
        this Element element)
    {
        return element.Document.GetElement(element.GetTypeId());
    }

    public static TElement Clone<TElement>(this TElement element)
        where TElement : Element
    {
        var document = element.Document;
        return document.GetElement(ElementTransformUtils
            .CopyElement(
                document,
                element.Id,
                XYZ.Zero).First()) as TElement;
    }
    
    
    public static void ChangeCurveLocation(
        this Element element, Curve curve)
    {
        if (!(element.Location is LocationCurve locationCurve))
            throw new ArgumentNullException($"{element.Name} is not curve-driven");
        locationCurve.Curve = curve;
    }


    public static void ChangePointLocation(
        this Element element, XYZ point)
    {
        if (!(element.Location is LocationPoint locationPoint))
            throw new ArgumentNullException($"{element.Name} is not point-driven");
        locationPoint.Point = point;
    }


    public static void Move(this Element element, XYZ translation)
    {
        var document = element.Document;
        ElementTransformUtils.MoveElement(document, element.Id, translation);
    }

    public static void Move(this IEnumerable<Element> elements, XYZ translation)
    {
        var document = elements.First().Document;
        var ids = elements.Select(e => e.Id).ToList();
        ElementTransformUtils.MoveElements(document, ids, translation);
    }


    public static IEnumerable<T> GetGeometryObjects<T>(
        this Element element,
        Options geometryOptions = null,
        GeometryRepresentation geometryRepresentation = GeometryRepresentation.Instance)
        where T : GeometryObject
    {
        if (element == null) throw new ArgumentNullException(nameof(element));
        geometryOptions ??= new Options();
        var geometryElements = new List<GeometryElement>()
        {
            element.get_Geometry(geometryOptions)
        };
        if (element is FamilyInstance familyInstance)
        {
            if (familyInstance.GetSubComponentIds().Any())
            {
                geometryElements.AddRange(familyInstance.GetSubComponentIds()
                    .Select(subComponentId => element.Document.GetElement(subComponentId))
                    .Select(subComponent => subComponent.get_Geometry(geometryOptions)));
            }
        }

        if (geometryElements.Any(x => x == null))
            return Enumerable.Empty<T>();

        return geometryElements.SelectMany(x => x.GetRootElements<T>(
            geometryRepresentation));
    }

    public static bool HasGeometryElement(this Element element) => element.get_Geometry(new Options()) is not null;
}