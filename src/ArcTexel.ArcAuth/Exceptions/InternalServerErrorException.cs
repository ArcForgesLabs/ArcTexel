namespace ArcTexel.ArcAuth.Exceptions;

public class InternalServerErrorException(string message) : ArcAuthException(500, message)
{

}
