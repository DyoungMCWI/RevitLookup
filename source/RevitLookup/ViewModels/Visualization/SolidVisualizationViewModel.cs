using Microsoft.Extensions.Logging;
using RevitLookup.Abstractions.Services.Presentation;
using RevitLookup.Abstractions.Services.Settings;
using RevitLookup.Abstractions.ViewModels.Visualization;
using RevitLookup.Core.Visualization;
using RevitLookup.Core.Visualization.Events;
using Color = System.Windows.Media.Color;

namespace RevitLookup.ViewModels.Visualization;

[UsedImplicitly]
public sealed partial class SolidVisualizationViewModel(
    ISettingsService settingsService,
    INotificationService notificationService,
    ILogger<SolidVisualizationViewModel> logger)
    : ObservableObject, ISolidVisualizationViewModel
{
    private readonly SolidVisualizationServer _server = new();

    [ObservableProperty] private double _scale = settingsService.VisualizationSettings.SolidSettings.Scale;
    [ObservableProperty] private double _transparency = settingsService.VisualizationSettings.SolidSettings.Transparency;

    [ObservableProperty] private Color _faceColor = settingsService.VisualizationSettings.SolidSettings.FaceColor;
    [ObservableProperty] private Color _edgeColor = settingsService.VisualizationSettings.SolidSettings.EdgeColor;

    [ObservableProperty] private bool _showFace = settingsService.VisualizationSettings.SolidSettings.ShowFace;
    [ObservableProperty] private bool _showEdge = settingsService.VisualizationSettings.SolidSettings.ShowEdge;

    public void RegisterServer(object solidObject)
    {
        if (solidObject is not Solid solid)
        {
            throw new ArgumentException($"Argument must be of type {nameof(Solid)}", nameof(solidObject));
        }

        UpdateShowFace(ShowFace);
        UpdateShowEdge(ShowEdge);

        UpdateFaceColor(FaceColor);
        UpdateEdgeColor(EdgeColor);

        UpdateTransparency(Transparency);
        UpdateScale(Scale);

        _server.RenderFailed += HandleRenderFailure;
        _server.Register(solid);
    }

    public void UnregisterServer()
    {
        _server.RenderFailed -= HandleRenderFailure;
        _server.Unregister();
    }

    private void HandleRenderFailure(object? sender, RenderFailedEventArgs args)
    {
        logger.LogError(args.ExceptionObject, "Render error");
        notificationService.ShowError("Render error", args.ExceptionObject);
    }

    partial void OnFaceColorChanged(Color value)
    {
        settingsService.VisualizationSettings.SolidSettings.FaceColor = value;
        UpdateFaceColor(value);
    }

    partial void OnEdgeColorChanged(Color value)
    {
        settingsService.VisualizationSettings.SolidSettings.EdgeColor = value;
        UpdateEdgeColor(value);
    }

    partial void OnTransparencyChanged(double value)
    {
        settingsService.VisualizationSettings.SolidSettings.Transparency = value;
        UpdateTransparency(value);
    }

    partial void OnScaleChanged(double value)
    {
        settingsService.VisualizationSettings.SolidSettings.Scale = value;
        UpdateScale(value);
    }

    partial void OnShowFaceChanged(bool value)
    {
        settingsService.VisualizationSettings.SolidSettings.ShowFace = value;
        UpdateShowFace(value);
    }

    partial void OnShowEdgeChanged(bool value)
    {
        settingsService.VisualizationSettings.SolidSettings.ShowEdge = value;
        UpdateShowEdge(value);
    }

    private void UpdateFaceColor(Color value)
    {
        _server.UpdateFaceColor(new Autodesk.Revit.DB.Color(value.R, value.G, value.B));
    }

    private void UpdateEdgeColor(Color value)
    {
        _server.UpdateEdgeColor(new Autodesk.Revit.DB.Color(value.R, value.G, value.B));
    }

    private void UpdateTransparency(double value)
    {
        _server.UpdateTransparency(value / 100);
    }

    private void UpdateScale(double value)
    {
        _server.UpdateScale(value / 100);
    }

    private void UpdateShowFace(bool value)
    {
        _server.UpdateFaceVisibility(value);
    }

    private void UpdateShowEdge(bool value)
    {
        _server.UpdateEdgeVisibility(value);
    }
}