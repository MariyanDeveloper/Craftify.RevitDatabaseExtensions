using Autodesk.Revit.DB;

namespace Craftify.RevitDatabaseExtensions.Views;

public static class ViewExtensions
{
	public static Transform GetTransform(this View view)
	{
		var transform = Transform.Identity;
		transform.BasisX = view.RightDirection;
		transform.BasisY = view.UpDirection;
		transform.BasisZ = view.ViewDirection;
		return transform;
	}
	public static Plane GetPlane(this View view)
	{
		return Plane.CreateByNormalAndOrigin(
			view.ViewDirection,
			view.Origin);
	}
}