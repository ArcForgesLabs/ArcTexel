using MessagePack;
using MessagePack.Formatters;
using MessagePack.Resolvers;
using ArcTexel.Parser.Collections;
using ArcTexel.Parser.Old.ArcV4;

namespace ArcTexel.Parser;

internal class ResolverV4 : IFormatterResolver
{
    private readonly ColorCollectionFormatterV4 _colorCollection = new();
    
    public static ResolverV4 Instance { get; } = new();
    
    public IMessagePackFormatter<T> GetFormatter<T>()
    {
        if (typeof(T) == typeof(ColorCollection))
        {
            return (IMessagePackFormatter<T>)_colorCollection;
        }

        return StandardResolverAllowPrivate.Instance.GetFormatter<T>();
    }
}

internal class ResolverV5 : IFormatterResolver
{
    private readonly ColorCollectionFormatter _colorCollection = new();
    
    public static ResolverV5 Instance { get; } = new();
    
    public IMessagePackFormatter<T> GetFormatter<T>()
    {
        if (typeof(T) == typeof(ColorCollection))
        {
            return (IMessagePackFormatter<T>)_colorCollection;
        }

        return StandardResolverAllowPrivate.Instance.GetFormatter<T>();
    }
}