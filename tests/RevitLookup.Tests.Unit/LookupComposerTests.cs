using LookupEngine;
using Nice3point.TUnit.Revit;
using Nice3point.TUnit.Revit.Executors;
using TUnit.Core.Executors;

namespace RevitLookup.Tests.Unit;

public sealed class RevitDocumentTests : RevitApiTest
{
    private static Document _documentFile = null!;

    [Before(Class)]
    [HookExecutor<RevitThreadExecutor>]
    public static async Task Setup()
    {
        var samplesDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Autodesk", $"Revit {Application.VersionNumber}", "Samples");
        var sampleFile = Directory.EnumerateFiles(samplesDirectory, "*.rvt", SearchOption.TopDirectoryOnly).FirstOrDefault();

        await Assert.That(sampleFile).IsNotNull();

        _documentFile = Application.OpenDocumentFile(sampleFile);
    }

    [After(Class)]
    [HookExecutor<RevitThreadExecutor>]
    public static void Cleanup()
    {
        _documentFile.Close(false);
    }

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task Decompose_ElementInstance_DecomposedMembers()
    {
        var elementsGroups = new FilteredElementCollector(_documentFile)
            .WhereElementIsNotElementType()
            .GroupBy(element => element.GetType())
            .ToArray();

        foreach (var group in elementsGroups)
        {
            foreach (var element in group.Take(1))
            {
                var decomposedObject = LookupComposer.Decompose(element);

                await Assert.That(decomposedObject.Members).IsNotEmpty();
            }
        }
    }

    [Test]
    [TestExecutor<RevitThreadExecutor>]
    public async Task Decompose_ElementTypes_DecomposedMembers()
    {
        var elementsGroups = new FilteredElementCollector(_documentFile)
            .WhereElementIsElementType()
            .GroupBy(element => element.GetType())
            .ToArray();

        foreach (var group in elementsGroups)
        {
            foreach (var element in group.Take(1))
            {
                var decomposedObject = LookupComposer.Decompose(element);

                await Assert.That(decomposedObject.Members).IsNotEmpty();
            }
        }
    }
}