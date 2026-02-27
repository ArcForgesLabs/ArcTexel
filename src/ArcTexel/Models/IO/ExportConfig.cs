using ArcTexel.AnimationRenderer.Core;
using ArcTexel.AnimationRenderer.FFmpeg;
using Drawie.Numerics;

namespace ArcTexel.Models.IO;

public class ExportConfig
{
   public VecI ExportSize { get; set; }
   public bool ExportAsSpriteSheet { get; set; } = false;
   public int SpriteSheetColumns { get; set; }
   public int SpriteSheetRows { get; set; }
   public IAnimationRenderer? AnimationRenderer { get; set; }
   
   public VectorExportConfig? VectorExportConfig { get; set; }
   public string ExportOutput { get; set; }
   public bool ExportFramesToFolder { get; set; }

   public ExportConfig(VecI exportSize)
   {
        ExportSize = exportSize;
   }
}
