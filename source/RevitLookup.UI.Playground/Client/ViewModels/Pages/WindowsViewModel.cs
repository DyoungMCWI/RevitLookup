using System.Numerics;
using System.Reflection;
using System.Windows.Media;
using Bogus;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using JetBrains.Annotations;
using RevitLookup.Abstractions.Services.Application;
using RevitLookup.UI.Framework.Views.Dashboard;
using RevitLookup.UI.Framework.Views.Decomposition;

namespace RevitLookup.UI.Playground.Client.ViewModels.Pages;

[UsedImplicitly]
public sealed partial class WindowsViewModel : ObservableObject
{
    [RelayCommand]
    private void ShowRevitLookupWindow()
    {
        Host.GetService<IUiOrchestratorService>()
            .Show<DashboardPage>();
    }

    [RelayCommand]
    private void ShowEventsWindow()
    {
        Host.GetService<IUiOrchestratorService>()
            .Show<EventsSummaryPage>();
    }

    [RelayCommand]
    private void ShowDecomposeColorsWindow()
    {
        var faker = new Faker();

        var colors = new List<Color>();
        for (var i = 0; i < faker.Random.Int(1, 666); i++)
        {
            colors.Add(Color.FromArgb(
                faker.Random.Byte(),
                faker.Random.Byte(),
                faker.Random.Byte(),
                faker.Random.Byte()
            ));
        }

        Host.GetService<IUiOrchestratorService>()
            .Decompose(colors)
            .Show<DecompositionSummaryPage>();
    }

    [RelayCommand]
    private void ShowDecomposeTextWindow()
    {
        var faker = new Faker();

        var strings = new List<string>();
        for (var i = 0; i < faker.Random.Int(1, 666); i++)
        {
            strings.Add(faker.Lorem.Sentence(666));
        }

        Host.GetService<IUiOrchestratorService>()
            .Decompose(strings)
            .Show<DecompositionSummaryPage>();
    }

    [RelayCommand]
    private void ShowDecomposeTypesWindow()
    {
        var assembly = Assembly.GetExecutingAssembly();
        Host.GetService<IUiOrchestratorService>()
            .Decompose(assembly.GetTypes())
            .Show<DecompositionSummaryPage>();
    }

    [RelayCommand]
    private void ShowDecomposeResolvedValuesWindow()
    {
        var faker = new Faker();

        var vectors = new List<Vector3>();
        for (var i = 0; i < faker.Random.Int(1, 666); i++)
        {
            vectors.Add(new Vector3(
                faker.Random.Float(),
                faker.Random.Float(),
                faker.Random.Float()
            ));
        }

        Host.GetService<IUiOrchestratorService>()
            .Decompose(vectors)
            .Show<DecompositionSummaryPage>();
    }
}