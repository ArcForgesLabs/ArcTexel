using ArcTexel.Helpers.Extensions;
using ArcTexel.Models.BrushEngine;
using ArcTexel.Parser;
using ArcTexel.ViewModels.Document;
using ArcTexel.ViewModels.Document.Nodes.Brushes;

namespace ArcTexel.Models.Serialization.Factories;

internal class BrushSerializationFactory : SerializationFactory<byte[], Brush>
{
    public override string DeserializationId { get; } = "ArcTexel.Brush";
    public override byte[] Serialize(Brush original)
    {
        ByteBuilder builder = new ByteBuilder();
        builder.AddString(original.Name);
        byte[] bytes = ArcParser.V5.Serialize(((DocumentViewModel)original.Document).ToSerializable());
        builder.AddInt(bytes.Length);
        builder.AddByteArray(bytes);

        return builder.Build();
    }

    public override bool TryDeserialize(object serialized, out Brush original,
        (string serializerName, string serializerVersion) serializerData)
    {
        if (serialized is byte[] bytes)
        {
            ByteExtractor extractor = new ByteExtractor(bytes);
            string name = extractor.GetString();
            int docLength = extractor.GetInt();
            byte[] docBytes = extractor.GetByteSpan(docLength).ToArray();
            var doc = ArcParser.V5.Deserialize(docBytes).ToDocument();
            original = new Brush(name, doc, "EMBEDDED", null)
            {
                IsDuplicable = false,
                IsReadOnly = true
            };

            return true;
        }

        original = null;
        return false;
    }
}
