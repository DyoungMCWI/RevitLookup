using System.Windows.Media;
using Bogus;
using CommunityToolkit.Mvvm.ComponentModel;
using JetBrains.Annotations;
using RevitLookup.Abstractions.ViewModels.Visualization;

namespace RevitLookup.UI.Playground.Mockups.ViewModels.Visualization;

[UsedImplicitly]
public sealed partial class MockCurveLoopVisualizationViewModel : ObservableObject, ICurveLoopVisualizationViewModel
{
    [ObservableProperty] private double _diameter;
    [ObservableProperty] private double _transparency;

    [ObservableProperty] private Color _surfaceColor;
    [ObservableProperty] private Color _curveColor;
    [ObservableProperty] private Color _directionColor;

    [ObservableProperty] private bool _showSurface;
    [ObservableProperty] private bool _showCurve;
    [ObservableProperty] private bool _showDirection;

    public MockCurveLoopVisualizationViewModel()
    {
        var faker = new Faker();

        MinThickness = 0;
        Transparency = faker.Random.Double(0, 100);
        Diameter = faker.Random.Double(0, 6);
        SurfaceColor = Color.FromRgb(faker.Random.Byte(), faker.Random.Byte(), faker.Random.Byte());
        CurveColor = Color.FromRgb(faker.Random.Byte(), faker.Random.Byte(), faker.Random.Byte());
        DirectionColor = Color.FromRgb(faker.Random.Byte(), faker.Random.Byte(), faker.Random.Byte());

        ShowSurface = faker.Random.Bool();
        ShowCurve = faker.Random.Bool();
        ShowDirection = faker.Random.Bool();
    }

    public double MinThickness { get; }

    public void RegisterServer(object curveOrEdge)
    {
    }

    public void UnregisterServer()
    {
    }
}