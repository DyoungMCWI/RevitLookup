using Nice3point.TUnit.Revit;
using Nice3point.TUnit.Revit.Executors;
using RevitLookup.Core;
using TUnit.Core.Executors;

namespace RevitLookup.Tests.Unit;

public sealed class RevitApiTests : RevitApiTest
{
    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task Parameters_Builtin_ShouldCreateAllCategories()
    {
#if NET
        var builtInParameters = Enum.GetValues<BuiltInParameter>();
#else
        var builtInParameters = Enum.GetValues(typeof(BuiltInParameter)).Cast<BuiltInParameter>().ToArray();
#endif

        foreach (var builtInParameter in builtInParameters)
        {
            var parameter = RevitShell.GetBuiltinParameter(builtInParameter);

            await Assert.That(parameter.Definition.Name).IsNotEmpty();
        }
    }
    
    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task Categories_Builtin_ShouldCreateAllCategories()
    {
#if NET
        var builtInCategories = Enum.GetValues<BuiltInCategory>();
#else
        var BuiltInCategory = Enum.GetValues(typeof(BuiltInCategory)).Cast<BuiltInCategory>().ToArray();
#endif

        foreach (var builtInCategory in builtInCategories)
        {
            var category = RevitShell.GetBuiltinCategory(builtInCategory);

            await Assert.That(category.Name).IsNotEmpty();
        }
    }
}