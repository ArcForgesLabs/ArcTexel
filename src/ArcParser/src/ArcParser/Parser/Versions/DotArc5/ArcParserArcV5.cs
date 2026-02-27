using MessagePack;

namespace ArcTexel.Parser.Versions.DotArc5;

internal partial class ArcParserArcV5 : ArcParser<Document>
{
    private static MessagePackSerializerOptions MessagePackOptions { get; } = MessagePackSerializerOptions.Standard
        .WithSecurity(MessagePackSecurity.UntrustedData)
        .WithResolver(ResolverV5.Instance);
}