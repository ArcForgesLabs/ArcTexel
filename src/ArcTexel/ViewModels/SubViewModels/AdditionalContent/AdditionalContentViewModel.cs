using ArcTexel.IdentityProvider;
using ArcTexel.Platform;

namespace ArcTexel.ViewModels.SubViewModels.AdditionalContent;

internal class AdditionalContentViewModel : SubViewModel<ViewModelMain>
{
    public IAdditionalContentProvider AdditionalContentProvider { get; }
    public AdditionalContentViewModel(ViewModelMain owner, IAdditionalContentProvider additionalContentProvider) : base(owner)
    {
        AdditionalContentProvider = additionalContentProvider;
    }
}
