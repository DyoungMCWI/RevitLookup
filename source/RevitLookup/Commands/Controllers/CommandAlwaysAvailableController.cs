using Autodesk.Revit.UI;

namespace RevitLookup.Commands.Controllers;

public sealed class CommandAlwaysAvailableController : IExternalCommandAvailability
{
    public bool IsCommandAvailable(UIApplication applicationData, CategorySet selectedCategories)
    {
        return true;
    }
}