using System.Windows.Input;
using Avalonia.Input;

namespace ArcTexel.Zoombox.Operations;

internal interface IDragOperation
{
    void Start(PointerEventArgs e);

    void Update(PointerEventArgs e);

    void Terminate();
}
