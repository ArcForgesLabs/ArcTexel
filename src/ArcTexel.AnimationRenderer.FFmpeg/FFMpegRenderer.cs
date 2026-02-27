using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;
using Drawie.Backend.Core.Surfaces;
using FFMpegCore;
using FFMpegCore.Arguments;
using FFMpegCore.Enums;
using FFMpegCore.Pipes;
using ArcTexel.AnimationRenderer.Core;
using Drawie.Backend.Core.Surfaces.ImageData;
using Drawie.Numerics;
using ArcTexel.OperatingSystem;

namespace ArcTexel.AnimationRenderer.FFmpeg;

public class FFMpegRenderer : IAnimationRenderer
{
    public int FrameRate { get; set; } = 60;
    public string OutputFormat { get; set; } = "mp4";
    public VecI Size { get; set; }
    public QualityPreset QualityPreset { get; set; } = QualityPreset.VeryHigh;

    public Regex FramerateRegex { get; } = new Regex(@"(\d+(?:\.\d+)?) fps", RegexOptions.Compiled);

    public async Task<bool> RenderAsync(List<Image> rawFrames, string outputPath, CancellationToken cancellationToken,
        Action<double>? progressCallback = null)
    {
        PrepareFFMpeg();

        string tempPath = Path.Combine(Path.GetTempPath(), "ArcTexel", "Rendering");
        Directory.CreateDirectory(tempPath);

        string paletteTempPath = Path.Combine(tempPath, "palette.png");

        try
        {
            List<ImgFrame> frames = new();

            foreach (var frame in rawFrames)
            {
                frames.Add(new ImgFrame(frame));
            }

            RawVideoPipeSource streamPipeSource = new(frames) { FrameRate = FrameRate, };

            if (RequiresPaletteGeneration())
            {
                GeneratePalette(streamPipeSource, paletteTempPath);
            }

            streamPipeSource = new(frames) { FrameRate = FrameRate, };

            var args = FFMpegArguments
                .FromPipeInput(streamPipeSource, options =>
                {
                    options.WithFramerate(FrameRate);
                });

            var outputArgs = GetProcessorForFormat(args, outputPath, paletteTempPath);
            TimeSpan totalTimeSpan = TimeSpan.FromSeconds(frames.Count / (float)FrameRate);
            var processor = outputArgs.CancellableThrough(cancellationToken);
            if (progressCallback != null)
            {
                processor = processor.NotifyOnProgress(progressCallback, totalTimeSpan);
            }

            var result = await processor.ProcessAsynchronously();

            DisposeStream(frames);

            return result;
        }
        finally
        {
            if (RequiresPaletteGeneration() && File.Exists(paletteTempPath))
            {
                File.Delete(paletteTempPath);
                string? paletteDir = Path.GetDirectoryName(paletteTempPath);
                if (!string.IsNullOrEmpty(paletteDir) && Directory.Exists(paletteDir))
                {
                    Directory.Delete(paletteDir);
                }
            }
        }
    }

    public bool Render(List<Image> rawFrames, string outputPath, CancellationToken cancellationToken,
        Action<double>? progressCallback)
    {
        PrepareFFMpeg();

        string tempPath = Path.Combine(Path.GetTempPath(), "ArcTexel", "Rendering");
        Directory.CreateDirectory(tempPath);

        string paletteTempPath = Path.Combine(tempPath, "palette.png");

        try
        {
            List<ImgFrame> frames = new();

            foreach (var frame in rawFrames)
            {
                frames.Add(new ImgFrame(frame));
            }

            RawVideoPipeSource streamPipeSource = new(frames) { FrameRate = FrameRate, };

            if (RequiresPaletteGeneration())
            {
                GeneratePalette(streamPipeSource, paletteTempPath);
            }

            streamPipeSource = new(frames) { FrameRate = FrameRate, };

            var args = FFMpegArguments
                .FromPipeInput(streamPipeSource, options =>
                {
                    options.WithFramerate(FrameRate);
                });

            var outputArgs = GetProcessorForFormat(args, outputPath, paletteTempPath);
            TimeSpan totalTimeSpan = TimeSpan.FromSeconds(frames.Count / (float)FrameRate);
            var processor = outputArgs.CancellableThrough(cancellationToken);
            if (progressCallback != null)
            {
                processor = processor.NotifyOnProgress(progressCallback, totalTimeSpan);
            }

            var result = processor.ProcessSynchronously();

            DisposeStream(frames);

            return result;
        }
        finally
        {
            if (RequiresPaletteGeneration() && File.Exists(paletteTempPath))
            {
                File.Delete(paletteTempPath);
                string? paletteDir = Path.GetDirectoryName(paletteTempPath);
                if (!string.IsNullOrEmpty(paletteDir) && Directory.Exists(paletteDir))
                {
                    Directory.Delete(paletteDir);
                }
            }
        }
    }

    public List<Frame> GetFrames(string inputPath, out double playbackFps)
    {
        PrepareFFMpeg();

        using var ms = new MemoryStream();
        if (!FFMpegArguments.FromFileInput(inputPath)
                .OutputToPipe(new StreamPipeSink(ms),
                    options =>
                        options.WithCustomArgument("-vsync 0")
                            .WithCustomArgument("-vcodec png")
                            .ForceFormat("image2pipe"))
                .ProcessSynchronously())
        {
            throw new InvalidOperationException("Failed to extract frames from video");
        }

        ms.Seek(0, SeekOrigin.Begin);
        List<Bitmap> frames = PipeUtil.ReadFramesFromPipe(ms);

        playbackFps = ExtractFramerateInfo(inputPath);

        return frames.Select(f => new Frame(f, 1)).ToList();
    }

    private double ExtractFramerateInfo(string inputPath)
    {
        var os = GetOperatingSystem();
        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName =
                Path.Combine(GetProcessDirectory(),
                    $"ThirdParty/{os.Name}/ffmpeg/ffmpeg"),
            Arguments = $"-i \"{inputPath}\"",
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using Process process = new() { StartInfo = startInfo };
        process.Start();
        process.WaitForExit();
        string info = process.StandardError.ReadToEnd();

        Match match = FramerateRegex.Match(info);
        if (match.Success && double.TryParse(match.Groups[1].Value, NumberStyles.Any, CultureInfo.InvariantCulture,
                out double fps))
        {
            return fps;
        }

        return 24;
    }

    private static void PrepareFFMpeg()
    {
        var os = GetOperatingSystem();
        string path = $"ThirdParty/{os.Name}/ffmpeg";

        string binaryPath = Path.Combine(GetProcessDirectory(), path);

        GlobalFFOptions.Configure(new FFOptions() { BinaryFolder = binaryPath });

        if (os.IsUnix)
        {
            MakeExecutableIfNeeded(binaryPath);
        }
    }

    private static void MakeExecutableIfNeeded(string binaryPath)
    {
        var os = GetOperatingSystem();
        string filePath = Path.Combine(binaryPath, "ffmpeg");

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("FFmpeg binary not found");
        }

        try
        {
            var process = os.ProcessUtility.Execute($"{filePath}", "-version");

            bool exited = process.WaitForExit(500);

            if (!exited)
            {
                throw new InvalidOperationException("Failed to perform FFmpeg check");
            }

            if (process.ExitCode == 0)
            {
                return;
            }

            os.ProcessUtility.Execute("chmod", $"+x {filePath}").WaitForExit(500);
        }
        catch (Exception)
        {
            os.ProcessUtility.Execute("chmod", $"+x {filePath}")
                .WaitForExit(500);
        }
    }

    private static string GetProcessDirectory()
    {
        string? processPath = Environment.ProcessPath;
        if (string.IsNullOrEmpty(processPath))
        {
            throw new InvalidOperationException("Process path is unavailable.");
        }

        string? processDir = Path.GetDirectoryName(processPath);
        if (string.IsNullOrEmpty(processDir))
        {
            throw new InvalidOperationException("Process directory is unavailable.");
        }

        return processDir;
    }

    private static IOperatingSystem GetOperatingSystem()
    {
        return IOperatingSystem.Current ?? throw new InvalidOperationException("Operating system is not registered.");
    }

    private void DisposeStream(List<ImgFrame> frames)
    {
        foreach (var frame in frames)
        {
            frame.Dispose();
        }
    }

    private FFMpegArgumentProcessor GetProcessorForFormat(FFMpegArguments args, string outputPath,
        string paletteTempPath)
    {
        return OutputFormat switch
        {
            "gif" => GetGifArguments(args, outputPath, paletteTempPath),
            "mp4" => GetMp4Arguments(args, outputPath),
            _ => throw new NotSupportedException($"Output format {OutputFormat} is not supported")
        };
    }

    private FFMpegArgumentProcessor GetGifArguments(FFMpegArguments args, string outputPath, string paletteTempPath)
    {
        return args
            .AddFileInput(paletteTempPath)
            .OutputToFile(outputPath, true, options =>
            {
                options.WithCustomArgument(
                        $"-filter_complex \"[0:v]fps={FrameRate},scale={Size.X}:{Size.Y}:flags=lanczos[x];[x][1:v]paletteuse\"") // Apply the palette
                    .WithCustomArgument($"-vsync 0"); // Ensure each input frame gets displayed exactly once
            });
    }

    private FFMpegArgumentProcessor GetMp4Arguments(FFMpegArguments args, string outputPath)
    {
        int qscale = QualityPreset switch
        {
            QualityPreset.VeryLow => 31,
            QualityPreset.Low => 25,
            QualityPreset.Medium => 19,
            QualityPreset.High => 10,
            QualityPreset.VeryHigh => 1,
            _ => 2
        };
        return args
            .OutputToFile(outputPath, true, options =>
            {
                options.WithFramerate(FrameRate)
                    .WithCustomArgument($"-qscale:v {qscale}")
                    .WithVideoCodec("mpeg4")
                    .ForcePixelFormat("yuv420p");
            });
    }

    private bool RequiresPaletteGeneration()
    {
        return OutputFormat == "gif";
    }

    private void GeneratePalette(IPipeSource imageStream, string path)
    {
        FFMpegArguments
            .FromPipeInput(imageStream, options =>
            {
                options.WithFramerate(FrameRate);
            })
            .OutputToFile(path, true, options =>
            {
                options
                    .WithCustomArgument($"-vf \"palettegen\"");
            })
            .ProcessSynchronously();
    }
}
