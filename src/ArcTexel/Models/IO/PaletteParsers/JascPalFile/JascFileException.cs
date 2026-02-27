using System.Runtime.Serialization;
using ArcTexel.Extensions.Exceptions;
using ArcTexel.UI.Common.Localization;

namespace ArcTexel.Models.IO.PaletteParsers.JascPalFile;


internal class JascFileException : RecoverableException
{
    public JascFileException() { }

    public JascFileException(LocalizedString displayMessage) : base(displayMessage) { }

    public JascFileException(LocalizedString displayMessage, Exception innerException) : base(displayMessage, innerException) { }

    public JascFileException(LocalizedString displayMessage, string exceptionMessage) : base(displayMessage, exceptionMessage) { }

    public JascFileException(LocalizedString displayMessage, string exceptionMessage, Exception innerException) : base(displayMessage, exceptionMessage, innerException) { }

    protected JascFileException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}
