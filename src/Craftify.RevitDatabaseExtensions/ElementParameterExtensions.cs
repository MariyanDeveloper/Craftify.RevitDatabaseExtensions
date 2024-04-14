using Autodesk.Revit.DB;

namespace Craftify.RevitDatabaseExtensions;

public static class ElementParameterExtensions
{
    public static Parameter GetParameterByBuiltInParameter(this Element element, BuiltInParameter parameter)
    {
        return element.get_Parameter(parameter);
    }

    public static double GetSillHeight(this FamilyInstance familyInstance)
    {
        return familyInstance.GetParameterByBuiltInParameter(
            BuiltInParameter.INSTANCE_SILL_HEIGHT_PARAM).AsDouble();
    }
}