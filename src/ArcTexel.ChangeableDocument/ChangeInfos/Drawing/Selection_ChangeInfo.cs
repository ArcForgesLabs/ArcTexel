using Drawie.Backend.Core.Vector;

namespace ArcTexel.ChangeableDocument.ChangeInfos.Drawing;

public record class Selection_ChangeInfo(VectorPath NewPath) : IChangeInfo;
