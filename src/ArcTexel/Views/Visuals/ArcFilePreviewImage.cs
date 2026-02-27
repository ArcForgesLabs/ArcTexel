using Avalonia;
using Avalonia.Threading;
using Drawie.Backend.Core;
using Drawie.Backend.Core.Surfaces;
using ArcTexel.Extensions.Exceptions;
using ArcTexel.Helpers;
using ArcTexel.Models;
using ArcTexel.Models.IO;
using Drawie.Numerics;
using ArcTexel.Parser;

namespace ArcTexel.Views.Visuals;

internal class ArcFilePreviewImage : TextureControl
{
    public static readonly StyledProperty<string> FilePathProperty =
        AvaloniaProperty.Register<ArcFilePreviewImage, string>(nameof(FilePath));

    public static readonly StyledProperty<VecI> ImageSizeProperty =
        AvaloniaProperty.Register<ArcFilePreviewImage, VecI>(nameof(VecI));

    public static readonly StyledProperty<bool> CorruptProperty =
        AvaloniaProperty.Register<ArcFilePreviewImage, bool>(nameof(Corrupt));

    public string FilePath
    {
        get => GetValue(FilePathProperty);
        set => SetValue(FilePathProperty, value);
    }

    public VecI ImageSize
    {
        get => GetValue(ImageSizeProperty);
        set => SetValue(ImageSizeProperty, value);
    }

    public bool Corrupt
    {
        get => GetValue(CorruptProperty);
        set => SetValue(CorruptProperty, value);
    }

    static ArcFilePreviewImage()
    {
        FilePathProperty.Changed.AddClassHandler<ArcFilePreviewImage>(OnFilePathChanged);
    }

    private void RunLoadImage()
    {
        var path = FilePath;

        Task.Run(() => LoadImage(path));
    }

    private async Task LoadImage(string path)
    {
        string fileExtension = Path.GetExtension(path);

        byte[] imageBytes;

        bool isArc = fileExtension == ".arc";
        if (isArc)
        {
            await using FileStream fileStream = File.OpenRead(path);
            imageBytes = await ArcParser.ReadPreviewAsync(fileStream);
        }
        else if (SupportedFilesHelper.IsExtensionSupported(fileExtension) &&
                 SupportedFilesHelper.IsRasterFormat(fileExtension))
        {
            imageBytes = await File.ReadAllBytesAsync(path);
        }
        else
        {
            return;
        }

        Dispatcher.UIThread.Post(() =>
        {
            try
            {
                var surface = LoadTexture(imageBytes);
                SetImage(surface);
            }
            catch (Exception e)
            {
                SetCorrupt();
            }
        });
    }

    private void SetImage(Texture? texture)
    {
        Texture = texture!;

        if (texture != null)
        {
            ImageSize = texture.Size;
        }
    }

    private static void OnFilePathChanged(ArcFilePreviewImage previewImage, AvaloniaPropertyChangedEventArgs args)
    {
        if (args.NewValue == null)
        {
            previewImage.Texture = null;
            return;
        }

        previewImage.RunLoadImage();
    }

    private Texture LoadTexture(byte[] textureBytes)
    {
        Texture loaded = null;

        try
        {
            loaded = Texture.Load(textureBytes);
        }
        catch (RecoverableException)
        {
            SetCorrupt();
        }

        if (loaded == null) //prevent crash
            return null;

        if (loaded.Size is { X: <= Constants.MaxPreviewWidth, Y: <= Constants.MaxPreviewHeight })
        {
            return loaded;
        }

        var downscaled = DownscaleSurface(loaded);
        loaded.Dispose();
        return downscaled;
    }

    private static Texture DownscaleSurface(Texture surface)
    {
        double factor = Math.Min(
            Constants.MaxPreviewWidth / (double)surface.Size.X,
            Constants.MaxPreviewHeight / (double)surface.Size.Y);

        var newSize = new VecI((int)(surface.Size.X * factor), (int)(surface.Size.Y * factor));

        var scaledBitmap = surface.Resize(newSize, FilterQuality.High);

        surface.Dispose();
        return scaledBitmap;
    }

    // TODO: This does not actually set the dot to gray
    void SetCorrupt()
    {
        Dispatcher.UIThread.Post(() => Corrupt = true);
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);
        Texture?.Dispose();
    }
}
