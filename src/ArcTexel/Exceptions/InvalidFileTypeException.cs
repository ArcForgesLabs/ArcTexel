using System.Runtime.Serialization;
using ArcTexel.Extensions.Exceptions;
using ArcTexel.UI.Common.Localization;

namespace ArcTexel.Exceptions;

internal class InvalidFileTypeException : RecoverableException
{
    public InvalidFileTypeException() { }

    public InvalidFileTypeException(LocalizedString displayMessage) : base(displayMessage) { }

    public InvalidFileTypeException(LocalizedString displayMessage, Exception innerException) : base(displayMessage, innerException) { }

    public InvalidFileTypeException(LocalizedString displayMessage, string exceptionMessage) : base(displayMessage, exceptionMessage) { }

    public InvalidFileTypeException(LocalizedString displayMessage, string exceptionMessage, Exception innerException) : base(displayMessage, exceptionMessage, innerException) { }

    protected InvalidFileTypeException(SerializationInfo info, StreamingContext context) : base(info, context) { }

}
