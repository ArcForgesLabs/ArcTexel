using System.Runtime.CompilerServices;

namespace ArcTexel.Extensions.Sdk.Bridge;

internal static partial class Native
{
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern string translate_key(string key);
}
