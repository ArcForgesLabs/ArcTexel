using System.IO;
using System.IO.Compression;
using System.Runtime.InteropServices;
using Avalonia.Media.Imaging;
using ChunkyImageLib;
using CommunityToolkit.Mvvm.ComponentModel;
using ArcTexel.Helpers.Extensions;
using Drawie.Backend.Core;
using Drawie.Backend.Core.Numerics;
using Drawie.Backend.Core.Surfaces;
using Drawie.Backend.Core.Surfaces.ImageData;
using Drawie.Backend.Core.Surfaces.PaintImpl;
using ArcTexel.Exceptions;
using ArcTexel.Extensions.Exceptions;
using ArcTexel.Helpers;
using Drawie.Numerics;
using ArcTexel.Parser;
using ArcTexel.Parser.Old.ArcV4;
using ArcTexel.UI.Common.Localization;
using ArcTexel.ViewModels.Document;
using Bitmap = Avalonia.Media.Imaging.Bitmap;
using BlendMode = Drawie.Backend.Core.Surfaces.BlendMode;

namespace ArcTexel.Models.IO;

internal class Importer : ObservableObject
{
    /// <summary>
    ///     Imports image from path and resizes it to given dimensions.
    /// </summary>
    /// <param name="path">Path of the image.</param>
    /// <param name="size">New size of the image.</param>
    /// <returns>WriteableBitmap of imported image.</returns>
    public static Surface? ImportImage(string path, VecI size)
    {
        if (!Path.Exists(path))
            throw new MissingFileException();

        Surface original;
        try
        {
            original = Surface.Load(path);
        }
        catch (Exception e) when (e is ArgumentException or FileNotFoundException)
        {
            throw new CorruptedFileException(e);
        }

        if (original.Size == size || size == VecI.NegativeOne)
        {
            return original;
        }

        Surface resized = original.ResizeNearestNeighbor(size);
        original.Dispose();
        return resized;
    }

    public static Bitmap ImportBitmap(string path)
    {
        try
        {
            return new Bitmap(path);
        }
        catch (NotSupportedException e)
        {
            throw new InvalidFileTypeException(
                new LocalizedString("FILE_EXTENSION_NOT_SUPPORTED", Path.GetExtension(path)), e);
        }
        /*catch (FileFormatException e) TODO: Not found in Avalonia
        {
            throw new CorruptedFileException("FAILED_TO_OPEN_FILE", e);
        }*/
        catch (Exception e)
        {
            throw new RecoverableException("ERROR_IMPORTING_IMAGE", e);
        }
    }

    public static DocumentViewModel ImportDocument(string path, bool associatePath = true)
    {
        try
        {
            using var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
            var arcDocument = ArcParser.DeserializeUsingCompatible(fileStream);

            var document = arcDocument switch
            {
                Document v5 => v5.ToDocument(),
                DocumentV4 v4 => v4.ToDocument()
                // TODO: Default handling
            };

            if (associatePath)
            {
                document.FullFilePath = path;
            }

            return document;
        }
        catch (DirectoryNotFoundException)
        {
            //TODO: Handle
            throw new RecoverableException();
        }
        catch (InvalidFileException e)
        {
            throw new CorruptedFileException("FAILED_TO_OPEN_FILE", e);
        }
        catch (OldFileFormatException e)
        {
            throw new CorruptedFileException("FAILED_TO_OPEN_FILE", e);
        }
    }

    public static DocumentViewModel ImportDocument(byte[] file, string? originalFilePath)
    {
        try
        {
            if (!ArcParser.TryGetCompatibleVersion(file, out var parser))
            {
                // TODO: Handle
                throw new RecoverableException();
            }

            var arcDocument = parser.Deserialize(file);

            var document = arcDocument switch
            {
                Document v5 => v5.ToDocument(),
                DocumentV4 v4 => v4.ToDocument()
                // TODO: Default handling
            };

            document.FullFilePath = originalFilePath;

            return document;
        }
        catch (InvalidFileException e)
        {
            throw new CorruptedFileException("FAILED_TO_OPEN_FILE", e);
        }
        catch (OldFileFormatException e)
        {
            throw new CorruptedFileException("FAILED_TO_OPEN_FILE", e);
        }
    }

    public static Surface GetPreviewSurface(string path)
    {
        if (!IsSupportedFile(path))
        {
            throw new InvalidFileTypeException(new LocalizedString("FILE_EXTENSION_NOT_SUPPORTED",
                Path.GetExtension(path)));
        }

        if (Path.GetExtension(path) != ".arc")
            return Surface.Load(path);

        using var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);

        return Surface.Load(ArcParser.ReadPreview(fileStream));
    }

    public static bool IsSupportedFile(string path)
    {
        return SupportedFilesHelper.IsSupported(path);
    }

    public static Surface LoadFromGZippedBytes(string path)
    {
        using FileStream compressedData = new(path, FileMode.Open);
        using GZipStream uncompressedData = new(compressedData, CompressionMode.Decompress);
        using MemoryStream resultBytes = new();
        uncompressedData.CopyTo(resultBytes);

        byte[] bytes = resultBytes.ToArray();
        int width = BitConverter.ToInt32(bytes, 0);
        int height = BitConverter.ToInt32(bytes, 4);

        ImageInfo info = new ImageInfo(width, height, ColorType.RgbaF16);
        IntPtr ptr = Marshal.AllocHGlobal(bytes.Length - 8);
        try
        {
            Marshal.Copy(bytes, 8, ptr, bytes.Length - 8);
            Pixmap map = new(info, ptr);
            DrawingSurface surface = DrawingSurface.Create(map);
            Surface finalSurface = new Surface(new VecI(width, height));
            using Paint paint = new() { BlendMode = BlendMode.Src };
            surface.Draw(finalSurface.DrawingSurface.Canvas, 0, 0, paint);
            return finalSurface;
        }
        finally
        {
            Marshal.FreeHGlobal(ptr);
        }
    }
}
