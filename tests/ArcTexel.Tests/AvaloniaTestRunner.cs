using Avalonia;
using Avalonia.Headless;
using Avalonia.Platform;
using Drawie.Interop.VulkanAvalonia;
using ArcTexel.Tests;

[assembly:TestFramework("ArcTexel.Tests.AvaloniaTestRunner", "ArcTexel.Tests")]
[assembly:CollectionBehavior(CollectionBehavior.CollectionPerAssembly, DisableTestParallelization = false, MaxParallelThreads = 1)]
[assembly: AvaloniaTestApplication(typeof(AvaloniaTestRunner))]
namespace ArcTexel.Tests
{
    public class AvaloniaTestRunner
    {
        public static AppBuilder BuildAvaloniaApp() => AppBuilder.Configure<App>()
            .UseHeadless(new AvaloniaHeadlessPlatformOptions()
            {
                UseHeadlessDrawing = true,
                FrameBufferFormat = PixelFormat.Rgba8888,
            });
    }
}
