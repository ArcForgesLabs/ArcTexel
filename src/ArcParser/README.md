[![Discord Server](https://badgen.net/badge/discord/join%20chat/7289DA?icon=discord)](https://discord.gg/psrCP35kdk)
[![Download](https://img.shields.io/badge/nuget-download-blue)](https://www.nuget.org/packages/ArcTexel.Parser/)
[![Downloads](https://img.shields.io/nuget/dt/ArcTexel.Parser)](https://www.nuget.org/packages/ArcTexel.Parser/)

<img src="https://user-images.githubusercontent.com/45312141/102829812-2e1c1c80-43e8-11eb-889c-0043e66e5fe5.png" width="700" />

---

## Getting started

Use `ArcParser.Deserialize()` to deserialize a document and `ArcParser.Serialize()` to serialize

```cs
using ArcTexel.Parser;


Document document = ArcParser.Deserialize("./arcFile.arc");

// Do some stuff with the document

ArcParser.Serialize(document, "./arcFile.arc");
```

## Installation

Package Manager Console:
```
Install-Package ArcTexel.Parser
```

.NET CLI:
```
dotnet add package ArcTexel.Parser
```

## SkiaSharp

We provide a package containing extensions for working with [SkiaSharp](https://github.com/mono/SkiaSharp)

### Example Usage

```cs
using ArcTexel.Parser.Skia;

// Get a SKImage from the png data of a IImageContainer (e.g. ImageLayer or ReferenceLayer)
SKImage image = layer.ToSKImage();
```

```cs
using ArcTexel.Parser.Skia;

// Encode the image data of the SKImage into the png data of a IImageContainer (e.g. ImageLayer or ReferenceLayer)
layer.FromSKImage(image);
```

### Installation

Package Manager Console:
```
Install-Package ArcTexel.Parser.Skia
```

.NET CLI:
```
dotnet add package ArcTexel.Parser.Skia
```

## Need Help?

You can find support here:

* Ask on our [Discord](https://discord.gg/qSRMYmq)
* Open a [Issue](https://github.com/ArcTexel/ArcParser/issues/new)

