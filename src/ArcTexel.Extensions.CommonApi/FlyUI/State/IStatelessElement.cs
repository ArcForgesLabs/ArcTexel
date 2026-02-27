namespace ArcTexel.Extensions.CommonApi.FlyUI.State;

public interface IStatelessElement<out TBuild> : ILayoutElement<TBuild>
{
    public ILayoutElement<TBuild> Build();
}
