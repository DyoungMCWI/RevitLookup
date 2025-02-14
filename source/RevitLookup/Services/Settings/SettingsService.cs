using System.IO;
using System.Text.Json;
using System.Windows.Media;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RevitLookup.Abstractions.Models.Settings;
using RevitLookup.Abstractions.Options;
using RevitLookup.Abstractions.Services.Settings;
using Wpf.Ui.Animations;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;
using Color = System.Windows.Media.Color;

namespace RevitLookup.Services.Settings;

public sealed class SettingsService(
    IOptions<FoldersOptions> foldersOptions,
    IOptions<JsonSerializerOptions> jsonOptions,
    ILogger<SettingsService> logger)
    : ISettingsService
{
    private GeneralSettings? _generalSettings;
    private RenderSettings? _renderSettings;

    public GeneralSettings GeneralSettings => _generalSettings ?? throw new InvalidOperationException("Settings is not loaded.");
    public RenderSettings RenderSettings => _renderSettings ?? throw new InvalidOperationException("Settings is not loaded.");

    public void SaveSettings()
    {
        SaveGeneralSettings();
        SaveRenderSettings();
    }

    public void LoadSettings()
    {
        LoadGeneralSettings();
        LoadRenderSettings();
    }

    private void SaveGeneralSettings()
    {
        var path = foldersOptions.Value.GeneralSettingsPath;
        if (!File.Exists(path)) Directory.CreateDirectory(Path.GetDirectoryName(path)!);

        var json = JsonSerializer.Serialize(_generalSettings, jsonOptions.Value);
        File.WriteAllText(path, json);
    }

    private void SaveRenderSettings()
    {
        var path = foldersOptions.Value.RenderSettingsPath;
        if (!File.Exists(path)) Directory.CreateDirectory(Path.GetDirectoryName(path)!);

        var json = JsonSerializer.Serialize(_renderSettings, jsonOptions.Value);
        File.WriteAllText(path, json);
    }

    private void LoadGeneralSettings()
    {
        var path = foldersOptions.Value.GeneralSettingsPath;
        if (!File.Exists(path))
        {
            ResetGeneralSettings();
            return;
        }

        try
        {
            using var config = File.OpenRead(path);
            _generalSettings = JsonSerializer.Deserialize<GeneralSettings>(config, jsonOptions.Value);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "General settings loading error");
        }
    }

    private void LoadRenderSettings()
    {
        var path = foldersOptions.Value.RenderSettingsPath;
        if (!File.Exists(path))
        {
            ResetRenderSettings();
            return;
        }

        try
        {
            using var config = File.OpenRead(path);
            _renderSettings = JsonSerializer.Deserialize<RenderSettings>(config, jsonOptions.Value);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "General settings loading error");
        }
    }

    public void ResetGeneralSettings()
    {
        _generalSettings = new GeneralSettings
        {
#if REVIT2024_OR_GREATER
            Theme = ApplicationTheme.Auto,
#else
            Theme = ApplicationTheme.Light,
#endif
            Background = WindowBackdropType.None,
            Transition = Transition.None,
            IncludeStatic = true,
            IncludeEvents = true,
            IncludeExtensions = true,
            UseHardwareRendering = true
        };
    }

    public void ResetRenderSettings()
    {
        _renderSettings = new RenderSettings
        {
            BoundingBoxSettings = new BoundingBoxVisualizationSettings
            {
                Transparency = 60,
                SurfaceColor = Colors.DodgerBlue,
                EdgeColor = Color.FromArgb(255, 30, 81, 255),
                AxisColor = Color.FromArgb(255, 255, 89, 30),
                ShowSurface = true,
                ShowEdge = true,
                ShowAxis = true
            },
            FaceSettings = new FaceVisualizationSettings
            {
                Transparency = 20,
                Extrusion = Context.Application.VertexTolerance * 12,
                MinExtrusion = Context.Application.VertexTolerance * 12,
                SurfaceColor = Colors.DodgerBlue,
                MeshColor = Color.FromArgb(255, 30, 81, 255),
                NormalVectorColor = Color.FromArgb(255, 255, 89, 30),
                ShowSurface = true,
                ShowMeshGrid = true,
                ShowNormalVector = true
            },
            MeshSettings = new MeshVisualizationSettings
            {
                Transparency = 20,
                Extrusion = Context.Application.VertexTolerance * 12,
                MinExtrusion = Context.Application.VertexTolerance * 12,
                SurfaceColor = Colors.DodgerBlue,
                MeshColor = Color.FromArgb(255, 30, 81, 255),
                NormalVectorColor = Color.FromArgb(255, 255, 89, 30),
                ShowSurface = true,
                ShowMeshGrid = true,
                ShowNormalVector = true
            },
            PolylineSettings = new PolylineVisualizationSettings
            {
                Transparency = 20,
                Diameter = 2,
                MinThickness = 0.1,
                SurfaceColor = Colors.DodgerBlue,
                CurveColor = Color.FromArgb(255, 30, 81, 255),
                DirectionColor = Color.FromArgb(255, 255, 89, 30),
                ShowSurface = true,
                ShowCurve = true,
                ShowDirection = true
            },
            SolidSettings = new SolidVisualizationSettings
            {
                Transparency = 20,
                Scale = 1,
                FaceColor = Colors.DodgerBlue,
                EdgeColor = Color.FromArgb(255, 30, 81, 255),
                ShowFace = true,
                ShowEdge = true
            },
            XyzSettings = new XyzVisualizationSettings
            {
                Transparency = 0,
                AxisLength = 6,
                MinAxisLength = 0.1,
                XColor = Color.FromArgb(255, 30, 227, 255),
                YColor = Color.FromArgb(255, 30, 144, 255),
                ZColor = Color.FromArgb(255, 30, 81, 255),
                ShowPlane = true,
                ShowXAxis = true,
                ShowYAxis = true,
                ShowZAxis = true
            }
        };
    }
}