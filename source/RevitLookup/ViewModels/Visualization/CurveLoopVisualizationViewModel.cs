// RevitLookup.ViewModels/Visualization/CurveLoopVisualizationViewModel.cs
using System.Collections.Generic;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using JetBrains.Annotations;
using RevitLookup.Abstractions.Services.Presentation;
using RevitLookup.Abstractions.Services.Settings;
using RevitLookup.Abstractions.ViewModels.Visualization;
using RevitLookup.Core.Visualization;
using RevitLookup.Core.Visualization.Events;
using Autodesk.Revit.DB;
using Microsoft.Extensions.Logging;
using System.Windows;
using Color = System.Windows.Media.Color;

namespace RevitLookup.ViewModels.Visualization
{
    [UsedImplicitly]
    public sealed partial class CurveLoopVisualizationViewModel : ObservableObject, ICurveLoopVisualizationViewModel
    {
        private readonly CurveLoopVisualizationServer _server = new();
        private readonly ISettingsService _settingsService;
        private readonly INotificationService _notificationService;
        private readonly ILogger<CurveLoopVisualizationViewModel> _logger;

        public CurveLoopVisualizationViewModel(
            ISettingsService settingsService,
            INotificationService notificationService,
            ILogger<CurveLoopVisualizationViewModel> logger)
        {
            _settingsService = settingsService;
            _notificationService = notificationService;
            _logger = logger;

            Diameter = _settingsService.VisualizationSettings.CurveLoopSettings.Diameter;
            Transparency = _settingsService.VisualizationSettings.CurveLoopSettings.Transparency;
            SurfaceColor = _settingsService.VisualizationSettings.CurveLoopSettings.SurfaceColor;
            CurveColor = _settingsService.VisualizationSettings.CurveLoopSettings.CurveColor;
            DirectionColor = _settingsService.VisualizationSettings.CurveLoopSettings.DirectionColor;
            ShowSurface = _settingsService.VisualizationSettings.CurveLoopSettings.ShowSurface;
            ShowCurve = _settingsService.VisualizationSettings.CurveLoopSettings.ShowCurve;
            ShowDirection = _settingsService.VisualizationSettings.CurveLoopSettings.ShowDirection;
        }

        public double MinThickness => _settingsService.VisualizationSettings.CurveLoopSettings.MinThickness;

        [ObservableProperty]
        private double _diameter;

        [ObservableProperty]
        private double _transparency;

        [ObservableProperty]
        private Color _surfaceColor;

        [ObservableProperty]
        private Color _curveColor;

        [ObservableProperty]
        private Color _directionColor;

        [ObservableProperty]
        private bool _showSurface;

        [ObservableProperty]
        private bool _showCurve;

        [ObservableProperty]
        private bool _showDirection;

        public void RegisterServer(object curveLoop)
        {
            if (curveLoop is CurveLoop loop)
            {
                Initialize();
                _server.RenderFailed += HandleRenderFailure;
                var allVertices = new List<XYZ>();
                foreach (var curve in loop)
                {
                    var collectionPts = curve.Tessellate().ToList();
                    foreach (var vertex in collectionPts)
                    {
                        if (allVertices.Any(pt => pt.IsAlmostEqualTo(vertex))) continue;

                        allVertices.Add(vertex);
                    }
                }

                if (!loop.IsOpen()) allVertices.Add(allVertices[0]);

                _server.Register(allVertices);
                return;
            }
            throw new ArgumentException("Unexpected CurveLoop type", nameof(curveLoop));
        }

        private void Initialize()
        {
            UpdateShowSurface(ShowSurface);
            UpdateShowCurve(ShowCurve);
            UpdateShowDirection(ShowDirection);

            UpdateSurfaceColor(SurfaceColor);
            UpdateCurveColor(CurveColor);
            UpdateDirectionColor(DirectionColor);

            UpdateTransparency(Transparency);
            UpdateDiameter(Diameter);
        }

        public void UnregisterServer()
        {
            _server.RenderFailed -= HandleRenderFailure;
            _server.Unregister();
        }

        private void HandleRenderFailure(object? sender, RenderFailedEventArgs args)
        {
            _logger.LogError(args.ExceptionObject, "Render error");
            _notificationService.ShowError("Render error", args.ExceptionObject);
        }

        partial void OnSurfaceColorChanged(Color value)
        {
            _settingsService.VisualizationSettings.CurveLoopSettings.SurfaceColor = value;
            UpdateSurfaceColor(value);
        }

        partial void OnCurveColorChanged(Color value)
        {
            _settingsService.VisualizationSettings.CurveLoopSettings.CurveColor = value;
            UpdateCurveColor(value);
        }

        partial void OnDirectionColorChanged(Color value)
        {
            _settingsService.VisualizationSettings.CurveLoopSettings.DirectionColor = value;
            UpdateDirectionColor(value);
        }

        partial void OnDiameterChanged(double value)
        {
            _settingsService.VisualizationSettings.CurveLoopSettings.Diameter = value;
            UpdateDiameter(value);
        }

        partial void OnTransparencyChanged(double value)
        {
            _settingsService.VisualizationSettings.CurveLoopSettings.Transparency = value;
            UpdateTransparency(value);
        }

        partial void OnShowSurfaceChanged(bool value)
        {
            _settingsService.VisualizationSettings.CurveLoopSettings.ShowSurface = value;
            UpdateShowSurface(value);
        }

        partial void OnShowCurveChanged(bool value)
        {
            _settingsService.VisualizationSettings.CurveLoopSettings.ShowCurve = value;
            UpdateShowCurve(value);
        }

        partial void OnShowDirectionChanged(bool value)
        {
            _settingsService.VisualizationSettings.CurveLoopSettings.ShowDirection = value;
            UpdateShowDirection(value);
        }

        private void UpdateSurfaceColor(Color value)
        {
            _server.UpdateSurfaceColor(new Autodesk.Revit.DB.Color(value.R, value.G, value.B));
        }

        private void UpdateCurveColor(Color value)
        {
            _server.UpdateCurveColor(new Autodesk.Revit.DB.Color(value.R, value.G, value.B));
        }

        private void UpdateDirectionColor(Color value)
        {
            _server.UpdateDirectionColor(new Autodesk.Revit.DB.Color(value.R, value.G, value.B));
        }

        private void UpdateDiameter(double value)
        {
            _server.UpdateDiameter(value / 12);
        }

        private void UpdateTransparency(double value)
        {
            _server.UpdateTransparency(value / 100);
        }

        private void UpdateShowSurface(bool value)
        {
            _server.UpdateSurfaceVisibility(value);
        }

        private void UpdateShowCurve(bool value)
        {
            _server.UpdateCurveVisibility(value);
        }

        private void UpdateShowDirection(bool value)
        {
            _server.UpdateDirectionVisibility(value);
        }
    }
}