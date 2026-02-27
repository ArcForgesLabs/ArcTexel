using System.Numerics;
using ArcTexel.Extensions.CommonApi.Utilities;

namespace ArcTexel.Extensions.CommonApi.FlyUI.Events;

public class NumberEventArgs : ElementEventArgs<NumberEventArgs>
{
    public double Value { get; }

    public NumberEventArgs(double value)
    {
        Value = value;
    }

    protected override void SerializeArgs(ByteWriter writer)
    {
        writer.WriteDouble(Value);
    }
}
