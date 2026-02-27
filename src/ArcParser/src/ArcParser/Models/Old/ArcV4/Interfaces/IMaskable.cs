namespace ArcTexel.Parser.Old.ArcV4.Interfaces;

/// <summary>
/// A maskable structure member
/// </summary>
public interface IMaskable : IStructureMember
{
    public Mask Mask { get; set; }
}