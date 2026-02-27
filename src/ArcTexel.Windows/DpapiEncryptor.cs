using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using ArcTexel.OperatingSystem;

namespace ArcTexel.Windows;

public class DpapiEncryptor : IEncryptor
{
    public byte[] Encrypt(byte[] data)
    {
        return ProtectedData.Protect(data, null, DataProtectionScope.CurrentUser);
    }

    public byte[] Decrypt(byte[] data)
    {
        return ProtectedData.Unprotect(data, null, DataProtectionScope.CurrentUser);
    }
}
