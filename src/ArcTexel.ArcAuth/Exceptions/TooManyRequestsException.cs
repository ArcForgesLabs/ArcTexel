namespace ArcTexel.ArcAuth.Exceptions;

public class TooManyRequestsException : Exception
{
    public double TimeLeft { get; }

    public TooManyRequestsException(string message, double timeLeft) : base(message)
    {
        TimeLeft = timeLeft;
    }
}
