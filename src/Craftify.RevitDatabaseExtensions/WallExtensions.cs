using Autodesk.Revit.DB;

namespace Craftify.RevitDatabaseExtensions;

public static class WallExtensions
{
    public static bool IsCurtain(this Wall wall) => wall.WallType.Kind == WallKind.Curtain;
}