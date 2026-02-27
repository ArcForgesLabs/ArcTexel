using Avalonia.Headless.XUnit;
using Drawie.Backend.Core;
using Drawie.Backend.Core.Bridge;
using Drawie.Backend.Core.ColorsImpl;
using Drawie.Backend.Core.ColorsImpl.Paintables;
using Drawie.Backend.Core.Surfaces.ImageData;
using Drawie.Backend.Core.Surfaces.PaintImpl;
using Drawie.Numerics;
using Drawie.Skia;
using DrawiEngine;
using Microsoft.Extensions.DependencyInjection;
using ArcTexel.ChangeableDocument.Changeables.Interfaces;
using ArcTexel.ChangeableDocument.Changes.NodeGraph;
using ArcTexel.Helpers;
using ArcTexel.Models.IO;
using ArcTexel.Models.Serialization;
using ArcTexel.Models.Serialization.Factories;
using ArcTexel.Models.Serialization.Factories.Paintables;
using ArcTexel.Parser.Skia.Encoders;
using ArcTexel.ViewModels.Document;

namespace ArcTexel.Tests;

public class SerializationTests : ArcTexelTest
{
    [Fact]
    public void TestThatAllFactoriesAreInServices()
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        var types = assemblies.Where(asm => !asm.FullName.Contains("Steamworks")).SelectMany(x => x.GetTypes())
            .Where(x => typeof(SerializationFactory).IsAssignableFrom(x) && x is { IsInterface: false, IsAbstract: false });
        
        var factoriesInAssemblies = types.ToList();

        var factoriesInActualServices = new ServiceCollection()
            .AddSerializationFactories()
            .Select(x => x.ImplementationType)
            .ToList();

        Assert.All(
            factoriesInAssemblies,
            expected => Assert.Contains(
                factoriesInActualServices,
                actual => actual == expected));
    }
    
    [Fact]
    public void TestThatAllPaintablesHaveFactories()
    {
        var allDrawiePaintableTypes = typeof(Paintable).Assembly.GetTypes()
            .Where(x => x.IsAssignableTo(typeof(Paintable))
                        && x is { IsAbstract: false, IsInterface: false }).ToList();

        var arcEditorAssemblyPaintables = typeof(SerializationFactory).Assembly.GetTypes()
            .Where(x => x.IsAssignableTo(typeof(Paintable))
                        && x is { IsAbstract: false, IsInterface: false }).ToList();

        var allPaintables = allDrawiePaintableTypes.Concat(arcEditorAssemblyPaintables).Distinct().ToList();

        var allFoundFactories = typeof(SerializationFactory).Assembly.GetTypes()
            .Where(x => x.IsAssignableTo(typeof(IPaintableSerializationFactory))
                        && x is { IsAbstract: false, IsInterface: false }).ToList();

        List<SerializationFactory> factories = new();
        QoiEncoder encoder = new QoiEncoder();
        SerializationConfig config = new SerializationConfig(encoder, ColorSpace.CreateSrgbLinear());

        foreach (var factoryType in allFoundFactories)
        {
            var factory = (SerializationFactory)Activator.CreateInstance(factoryType);
            factories.Add(factory);
        }

        foreach (var type in allPaintables)
        {
            var factory = factories.FirstOrDefault(x => x.OriginalType == type);
            Assert.NotNull(factory);
        }
    }

    [Fact]
    public void TestTexturePaintableFactory()
    {
        Texture texture = new Texture(new VecI(32, 32));
        texture.DrawingSurface.Canvas.DrawCircle(16, 16, 16, new Paint() { Color = Colors.Red, BlendMode = Drawie.Backend.Core.Surfaces.BlendMode.Src });
        TexturePaintable paintable = new TexturePaintable(texture);
        TexturePaintableSerializationFactory factory = new TexturePaintableSerializationFactory();
        factory.Config = new SerializationConfig(new QoiEncoder(), ColorSpace.CreateSrgbLinear());
        var serialized = factory.Serialize(paintable);
        var deserialized = (TexturePaintable)factory.Deserialize(serialized, default);

        Assert.NotNull(deserialized);
        var deserializedImage = deserialized.Image;
        Assert.NotNull(deserializedImage);
        Assert.Equal(texture.Size, deserializedImage.Size);
        for (int y = 0; y < texture.Size.Y; y++)
        {
            for (int x = 0; x < texture.Size.X; x++)
            {
                Color originalPixel = texture.GetSrgbPixel(new VecI(x, y));
                Color deserializedPixel = deserializedImage.GetSrgbPixel(new VecI(x, y));
                Assert.Equal(originalPixel, deserializedPixel);
            }
        }
    }

    [AvaloniaTheory]
    [InlineData("Fibi")]
    [InlineData("Pond")]
    [InlineData("SmlPxlCircShadWithMask")]
    [InlineData("SmallPixelArtCircleShadow")]
    [InlineData("SmlPxlCircShadWithMaskClipped")]
    [InlineData("SmlPxlCircShadWithMaskClippedInFolder")]
    [InlineData("VectorRectangleClippedToCircle")]
    [InlineData("VectorRectangleClippedToCircleShadowFilter")]
    [InlineData("VectorRectangleClippedToCircleMasked")]
    public void TestThatDeserializationOfSampleFilesDoesntThrow(string fileName)
    {
        string arcFile = Path.Combine("TestFiles", "RenderTests", fileName + ".arc");
        var document = Importer.ImportDocument(arcFile);
        Assert.NotNull(document);
    }
}