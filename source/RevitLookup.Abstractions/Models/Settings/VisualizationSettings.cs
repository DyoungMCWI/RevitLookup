using System.Text.Json.Serialization;
using System.Windows.Media;

namespace RevitLookup.Abstractions.Models.Settings;

/// <summary>
///     Schema for visualization settings.
/// </summary>
[Serializable]
public sealed class VisualizationSettings
{
    [JsonPropertyName("BoundingBoxSettings")] public required BoundingBoxVisualizationSettings BoundingBoxSettings { get; set; }
    [JsonPropertyName("FaceSettings")] public required FaceVisualizationSettings FaceSettings { get; set; }
    [JsonPropertyName("MeshSettings")] public required MeshVisualizationSettings MeshSettings { get; set; }
    [JsonPropertyName("PolylineSettings")] public required PolylineVisualizationSettings PolylineSettings { get; set; }
    [JsonPropertyName("CurveLoopSettings")] public required CurveLoopVisualizationSettings CurveLoopSettings { get; set; }
    [JsonPropertyName("SolidSettings")] public required SolidVisualizationSettings SolidSettings { get; set; }
    [JsonPropertyName("XyzSettings")] public required XyzVisualizationSettings XyzSettings { get; set; }
}

/// <summary>
///     Schema for bounding box visualization settings.
/// </summary>
[Serializable]
public class BoundingBoxVisualizationSettings
{
    [JsonPropertyName("Transparency")] public double Transparency { get; set; }

    [JsonPropertyName("SurfaceColor")] public Color SurfaceColor { get; set; }
    [JsonPropertyName("EdgeColor")] public Color EdgeColor { get; set; }
    [JsonPropertyName("AxisColor")] public Color AxisColor { get; set; }

    [JsonPropertyName("ShowSurface")] public bool ShowSurface { get; set; }
    [JsonPropertyName("ShowEdge")] public bool ShowEdge { get; set; }
    [JsonPropertyName("ShowAxis")] public bool ShowAxis { get; set; }
}

/// <summary>
///     Schema for face visualization settings.
/// </summary>
[Serializable]
public class FaceVisualizationSettings
{
    [JsonPropertyName("Transparency")] public double Transparency { get; set; }
    [JsonPropertyName("Extrusion")] public double Extrusion { get; set; }
    [JsonPropertyName("MinExtrusion")] public double MinExtrusion { get; set; }

    [JsonPropertyName("SurfaceColor")] public Color SurfaceColor { get; set; }
    [JsonPropertyName("MeshColor")] public Color MeshColor { get; set; }
    [JsonPropertyName("NormalVectorColor")] public Color NormalVectorColor { get; set; }

    [JsonPropertyName("ShowSurface")] public bool ShowSurface { get; set; }
    [JsonPropertyName("ShowMeshGrid")] public bool ShowMeshGrid { get; set; }
    [JsonPropertyName("ShowNormalVector")] public bool ShowNormalVector { get; set; }
}

/// <summary>
///     Schema for mesh visualization settings.
/// </summary>
[Serializable]
public class MeshVisualizationSettings
{
    [JsonPropertyName("Transparency")] public double Transparency { get; set; }
    [JsonPropertyName("Extrusion")] public double Extrusion { get; set; }
    [JsonPropertyName("MinExtrusion")] public double MinExtrusion { get; set; }

    [JsonPropertyName("SurfaceColor")] public Color SurfaceColor { get; set; }
    [JsonPropertyName("MeshColor")] public Color MeshColor { get; set; }
    [JsonPropertyName("NormalVectorColor")] public Color NormalVectorColor { get; set; }

    [JsonPropertyName("ShowSurface")] public bool ShowSurface { get; set; }
    [JsonPropertyName("ShowMeshGrid")] public bool ShowMeshGrid { get; set; }
    [JsonPropertyName("ShowNormalVector")] public bool ShowNormalVector { get; set; }
}

/// <summary>
///     Schema for polyline visualization settings.
/// </summary>
[Serializable]
public class PolylineVisualizationSettings
{
    [JsonPropertyName("Transparency")] public double Transparency { get; set; }
    [JsonPropertyName("Diameter")] public double Diameter { get; set; }
    [JsonPropertyName("MinThickness")] public double MinThickness { get; set; }

    [JsonPropertyName("SurfaceColor")] public Color SurfaceColor { get; set; }
    [JsonPropertyName("CurveColor")] public Color CurveColor { get; set; }
    [JsonPropertyName("DirectionColor")] public Color DirectionColor { get; set; }

    [JsonPropertyName("ShowSurface")] public bool ShowSurface { get; set; }
    [JsonPropertyName("ShowCurve")] public bool ShowCurve { get; set; }
    [JsonPropertyName("ShowDirection")] public bool ShowDirection { get; set; }
}

/// <summary>
///     Schema for CurveLoop visualization settings.
/// </summary>
[Serializable]
public class CurveLoopVisualizationSettings
{
    [JsonPropertyName("Transparency")] public double Transparency { get; set; }
    [JsonPropertyName("Diameter")] public double Diameter { get; set; }
    [JsonPropertyName("MinThickness")] public double MinThickness { get; set; }

    [JsonPropertyName("SurfaceColor")] public Color SurfaceColor { get; set; }
    [JsonPropertyName("CurveColor")] public Color CurveColor { get; set; }
    [JsonPropertyName("DirectionColor")] public Color DirectionColor { get; set; }

    [JsonPropertyName("ShowSurface")] public bool ShowSurface { get; set; }
    [JsonPropertyName("ShowCurve")] public bool ShowCurve { get; set; }
    [JsonPropertyName("ShowDirection")] public bool ShowDirection { get; set; }
}

/// <summary>
///     Schema for solid visualization settings.
/// </summary>
[Serializable]
public sealed class SolidVisualizationSettings
{
    [JsonPropertyName("Transparency")] public double Transparency { get; set; }
    [JsonPropertyName("Scale")] public double Scale { get; set; }

    [JsonPropertyName("FaceColor")] public Color FaceColor { get; set; }
    [JsonPropertyName("EdgeColor")] public Color EdgeColor { get; set; }

    [JsonPropertyName("ShowFace")] public bool ShowFace { get; set; }
    [JsonPropertyName("ShowEdge")] public bool ShowEdge { get; set; }
}

/// <summary>
///     Schema for XYZ visualization settings.
/// </summary>
[Serializable]
public class XyzVisualizationSettings
{
    [JsonPropertyName("Transparency")] public double Transparency { get; set; }
    [JsonPropertyName("AxisLength")] public double AxisLength { get; set; }
    [JsonPropertyName("MinAxisLength")] public double MinAxisLength { get; set; }

    [JsonPropertyName("XColor")] public Color XColor { get; set; }
    [JsonPropertyName("YColor")] public Color YColor { get; set; }
    [JsonPropertyName("ZColor")] public Color ZColor { get; set; }

    [JsonPropertyName("ShowPlane")] public bool ShowPlane { get; set; }
    [JsonPropertyName("ShowXAxis")] public bool ShowXAxis { get; set; }
    [JsonPropertyName("ShowYAxis")] public bool ShowYAxis { get; set; }
    [JsonPropertyName("ShowZAxis")] public bool ShowZAxis { get; set; }
}