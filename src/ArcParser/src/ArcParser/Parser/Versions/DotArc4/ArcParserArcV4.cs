using MessagePack;
using ArcTexel.Parser.Old.ArcV4;

namespace ArcTexel.Parser.Versions.DotArc4;

internal partial class ArcParserArcV4 : ArcParser<DocumentV4>
{
    private static MessagePackSerializerOptions MessagePackOptions { get; } = MessagePackSerializerOptions.Standard
        .WithSecurity(MessagePackSecurity.UntrustedData)
        .WithResolver(ResolverV4.Instance);
}