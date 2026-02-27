namespace ArcTexel.ArcAuth.Exceptions;

public class ArcAuthException : Exception
{
    public int StatusCode { get; }

    public ArcAuthException(int statusCode, string message) : base(message)
    {
        StatusCode = statusCode;
    }
}
