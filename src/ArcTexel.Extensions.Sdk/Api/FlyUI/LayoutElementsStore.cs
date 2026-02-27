using ArcTexel.Extensions.CommonApi.FlyUI;

namespace ArcTexel.Extensions.Sdk.Api.FlyUI;

internal static class LayoutElementsStore
{
    public static Dictionary<int, ILayoutElement<ControlDefinition>> LayoutElements { get; } = new();

    public static void AddElement(int internalId, ILayoutElement<ControlDefinition> element)
    {
        LayoutElements.Add(internalId, element);
    }

    public static void RemoveElement(int internalId)
    {
        LayoutElements.Remove(internalId);
    }
}
