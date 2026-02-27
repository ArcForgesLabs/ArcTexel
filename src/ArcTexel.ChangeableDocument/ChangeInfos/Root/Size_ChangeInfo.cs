using Drawie.Backend.Core.Numerics;
using Drawie.Numerics;

namespace ArcTexel.ChangeableDocument.ChangeInfos.Root;

public record class Size_ChangeInfo(VecI Size, double VerticalSymmetryAxisX, double HorizontalSymmetryAxisY) : IChangeInfo;
