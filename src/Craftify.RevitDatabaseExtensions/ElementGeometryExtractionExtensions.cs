using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using Craftify.Geometry.Enums;
using Craftify.Geometry.Extensions;
using Craftify.RevitDatabaseExtensions.Exceptions;

namespace Craftify.RevitDatabaseExtensions;

public static class ElementGeometryExtractionExtensions
{
    public static IEnumerable<TGeometryObject> GetGeometryObjects<TGeometryObject>(
        this Element element,
        Options? geometryOptions = null,
        GeometryRepresentation geometryRepresentation = GeometryRepresentation.Instance)
        where TGeometryObject : GeometryObject
    {
        ElementArgumentNullException.ThrowIfElementNull(element);
        geometryOptions ??= new Options();
        var geometryElements = new List<GeometryElement>()
        {
            element.get_Geometry(geometryOptions)
        };
        if (element is FamilyInstance familyInstance)
        {
            geometryElements
                .AppendFamilyInstanceGeometryElementsIfHasNested(familyInstance, geometryOptions);
        }
        return geometryElements
            .Where(g => g is not null)
            .SelectMany(x => x.ExtractRootGeometries<TGeometryObject>(
                geometryRepresentation));
    }
    public static IEnumerable<Solid> GetSolids(this Element element, Options? geometryOptions = null)
    {
        ElementArgumentNullException.ThrowIfElementNull(element);
        geometryOptions ??= new Options()
        {
            DetailLevel = ViewDetailLevel.Fine
        };
        return element.GetGeometryObjects<Solid>(geometryOptions: geometryOptions)
            .Where(s => s.HasVolume());
    }

    public static Curve GetPlacementCurve(this Element element)
    {
        ElementArgumentNullException.ThrowIfElementNull(element);
        ElementArgumentNullException.ThrowIfElementNotCurveDriven(element);
        return ((LocationCurve)element.Location).Curve;
    }

    public static XYZ GetPlacementPoint(this Element element)
    {
        ElementArgumentNullException.ThrowIfElementNull(element);
        ElementArgumentNullException.ThrowIfElementNotPointDriven(element);
        return ((LocationPoint)element.Location).Point;
    }

    private static void AppendFamilyInstanceGeometryElementsIfHasNested(
        this List<GeometryElement> geometryElements,
        FamilyInstance familyInstance,
        Options options)
    {
        if (familyInstance.GetSubComponentIds().Any() is false)
        {
            return;
        }
        var familyInstanceGeometryElements = familyInstance
            .GetGeometryElementsIncludingNestedComponents(options);
        geometryElements.AddRange(familyInstanceGeometryElements);
    }
    private static IEnumerable<GeometryElement> GetGeometryElementsIncludingNestedComponents(
        this FamilyInstance familyInstance,
        Options geometryOptions)
    {
        var familyInstanceGeometryElements = familyInstance
            .GetSubComponentIds()
            .Select(subComponentId => familyInstance.Document.GetElement(subComponentId))
            .Select(subComponent => subComponent.get_Geometry(geometryOptions));
        return familyInstanceGeometryElements;
    }
}