using ArcTexel.Extensions.Sdk;
using ArcTexel.Extensions.Sdk.Utilities;

namespace CGlueTestLib;

public static class Program
{
    public static void Main()
    {
        ArcTexelApi api = new ArcTexelApi(); // to prevent linker from removing the assembly
    }
}
