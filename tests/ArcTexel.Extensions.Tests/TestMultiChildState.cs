using System.Collections;
using ArcTexel.Extensions.CommonApi.FlyUI.Events;
using ArcTexel.Extensions.FlyUI.Elements;

namespace ArcTexel.Extensions.Test;

public class TestMultiChildState : State
{
    private LayoutElement[] rows = Array.Empty<LayoutElement>();
    public LayoutElement[] Rows => rows;

    public override LayoutElement BuildElement()
    {
        return new Column(
            new Button(new Text("Add row"), OnClick),
            new Row(rows));
    }

    private void OnClick(ElementEventArgs args)
    {
        SetState(() =>
        {
            rows = rows.Append(new Text("Row " + rows.Length)).ToArray();
        });
    }
}
