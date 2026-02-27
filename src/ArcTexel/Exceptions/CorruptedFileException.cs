using System.Runtime.Serialization;
using ArcTexel.Extensions.Exceptions;
using ArcTexel.UI.Common.Localization;

namespace ArcTexel.Exceptions;

[Serializable]
internal class CorruptedFileException : RecoverableException
{
    public CorruptedFileException() : base("FAILED_TO_OPEN_FILE") { }

    public CorruptedFileException(Exception innerException) : base("FAILED_TO_OPEN_FILE", innerException) { }

    public CorruptedFileException(LocalizedString displayMessage) : base(displayMessage) { }

    public CorruptedFileException(LocalizedString displayMessage, Exception innerException) : base(displayMessage, innerException) { }

    public CorruptedFileException(LocalizedString displayMessage, string exceptionMessage) : base(displayMessage, exceptionMessage) { }

    public CorruptedFileException(LocalizedString displayMessage, string exceptionMessage, Exception innerException) : base(displayMessage, exceptionMessage, innerException) { }

    protected CorruptedFileException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}
