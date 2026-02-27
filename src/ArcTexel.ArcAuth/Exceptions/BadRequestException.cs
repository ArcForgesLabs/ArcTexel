namespace ArcTexel.ArcAuth.Exceptions;

public class BadRequestException(string message) : ArcAuthException(400, message)
{

}
