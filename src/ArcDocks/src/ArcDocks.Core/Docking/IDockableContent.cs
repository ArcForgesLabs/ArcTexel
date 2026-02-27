using System.ComponentModel;
using System.Windows.Input;

namespace ArcDocks.Core.Docking;

public interface IDockableContent
{
    string Id { get;  }
    string Title { get;  }
    bool CanFloat { get; }
    bool CanClose { get; }
    TabCustomizationSettings TabCustomizationSettings { get; }
}