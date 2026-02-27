using System.Collections.Generic;

namespace ArcTexel.Parser;

public interface IKeyFrameChildrenContainer 
{
    public List<IKeyFrame> Children { get; }
}