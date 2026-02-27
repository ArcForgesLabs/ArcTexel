using ArcTexel.Extensions.CommonApi.Palettes;
using ArcTexel.Extensions.Sdk.Bridge;

namespace ArcTexel.Extensions.Sdk.Api.Palettes;

public class PalettesProvider : IPalettesProvider
{
    public void RegisterDataSource(PaletteListDataSource dataSource)
    {
        Interop.RegisterDataSource(dataSource);
    }
}
