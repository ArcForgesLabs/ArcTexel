using System;

namespace ArcTexel.Parser;

[Serializable]
public class InvalidFileException : Exception
{
    public IArcDocument Document { get; internal set; }
    
    public IArcParser Parser { get; internal set; }

    public InvalidFileException() { }
    public InvalidFileException(string message) : base(message) { }
    public InvalidFileException(string message, Exception inner) : base(message, inner) { }
    protected InvalidFileException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}
