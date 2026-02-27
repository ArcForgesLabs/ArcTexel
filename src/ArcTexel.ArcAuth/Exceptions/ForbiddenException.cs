namespace ArcTexel.ArcAuth.Exceptions;

public class ForbiddenException(string message) : ArcAuthException(403, message)
{

}
