namespace ArcTexel.Models.DocumentModels.UpdateableChangeExecutors.Features;

public interface ITextOverlayEvents :IExecutorFeature
{
    public void OnTextChanged(string text);
}
