using ArcTexel.IdentityProvider;
using ArcTexel.IdentityProvider.ArcAuth;

namespace ArcTexel.Platform.MSStore;

public sealed class MicrosoftStorePlatform : IPlatform
{
    public MicrosoftStorePlatform(string extensionsPath, string apiUrl, string? apiKey)
    {
        var provider = new ArcAuthIdentityProvider(apiUrl, apiKey);
        IdentityProvider = provider;
        AdditionalContentProvider = new MSAdditionalContentProvider(extensionsPath, provider);
    }

    public string Id { get; } = "ms-store";
    public string Name => "Microsoft Store";

    public bool PerformHandshake()
    {
        IdentityProvider?.Initialize();
        return true;
    }

    public void Update()
    {
    }

    public IAdditionalContentProvider? AdditionalContentProvider { get; }
    public IIdentityProvider? IdentityProvider { get; }
}
