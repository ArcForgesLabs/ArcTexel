using System.Collections.Generic;
using ArcTexel.Models.Position;

namespace ArcTexel.Models.DocumentModels;
#nullable enable
internal class DocumentState
{
    public Dictionary<Guid, ViewportInfo> Viewports { get; set; } = new();
}
