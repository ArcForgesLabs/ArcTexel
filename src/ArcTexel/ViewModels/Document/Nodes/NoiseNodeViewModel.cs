using ArcTexel.ChangeableDocument.Changeables.Graph.Nodes;
using ArcTexel.UI.Common.Fonts;
using ArcTexel.ViewModels.Nodes;
using ArcTexel.ViewModels.Nodes.Properties;

namespace ArcTexel.ViewModels.Document.Nodes;

[NodeViewModel("NOISE_NODE", "IMAGE", ArcPerfectIcons.Noise)]
internal class NoiseNodeViewModel : NodeViewModel<NoiseNode>
{
    private GenericEnumPropertyViewModel Type { get; set; }
    private GenericEnumPropertyViewModel VoronoiFeature { get; set; }
    private NodePropertyViewModel Randomness { get; set; }
    private NodePropertyViewModel AngleOffset { get; set; }

    public override void OnInitialized()
    {
        Type = FindInputProperty("NoiseType") as GenericEnumPropertyViewModel;
        VoronoiFeature = FindInputProperty("VoronoiFeature") as GenericEnumPropertyViewModel;
        Randomness = FindInputProperty("Randomness");
        AngleOffset = FindInputProperty("AngleOffset");

        Type.ValueChanged += (_, _) => UpdateInputVisibility();
        UpdateInputVisibility();
    }

    private void UpdateInputVisibility()
    {
        if (Type.Value is not NoiseType type)
            return;
        
        Randomness.IsVisible = type == NoiseType.Voronoi;
        VoronoiFeature.IsVisible = type == NoiseType.Voronoi;
        AngleOffset.IsVisible = type == NoiseType.Voronoi;
    }
}
