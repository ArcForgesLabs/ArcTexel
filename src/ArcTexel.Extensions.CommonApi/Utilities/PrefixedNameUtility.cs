namespace ArcTexel.Extensions.CommonApi.Utilities;

public static class PrefixedNameUtility
{
    /// <summary>
    ///     Converts a preference name to a full name with the extension unique name. It is relative to ArcTexel, so
    /// any preference without a prefix is a ArcTexel preference.
    /// </summary>
    /// <param name="uniqueName">Unique name of the extension.</param>
    /// <param name="name">Name of the preference.</param>
    /// <returns>Full name of the preference.</returns>
    public static string ToArcTexelRelativePreferenceName(string uniqueName, string name)
    {
        string[] splitted = name.Split(":");

        string finalName = $"{uniqueName}:{name}";

        if (splitted.Length == 2)
        {
            finalName = name;

            if (splitted[0].Equals("arctexel", StringComparison.CurrentCultureIgnoreCase))
            {
                finalName = splitted[1];
            }
        }

        return finalName;
    }

    /// <summary>
    ///    It is a reverse of <see cref="ToArcTexelRelativePreferenceName"/>. It converts a full preference name to a relative name.
    /// Preferences owned by the extension will not have any prefix, while ArcTexel preferences will have "arctexel:" prefix.
    /// </summary>
    /// <param name="extensionUniqueName">Unique name of the extension.</param>
    /// <param name="preferenceName">Full name of the preference.</param>
    /// <returns>Relative name of the preference.</returns>
    public static string ToExtensionRelativeName(string extensionUniqueName, string preferenceName)
    {
        if (preferenceName.StartsWith(extensionUniqueName))
        {
            return preferenceName[(extensionUniqueName.Length + 1)..];
        }

        if (preferenceName.Split(":").Length == 1)
        {
            return $"arctexel:{preferenceName}";
        }

        return preferenceName;
    }

    public static string ToCommandUniqueName(string extensionUniqueName, string commandUniqueName, bool allowAlreadyPrefixed)
    {
        if (commandUniqueName.Contains(':'))
        {
            if (allowAlreadyPrefixed)
            {
                return commandUniqueName;
            }

            throw new ArgumentException($"Command name '{commandUniqueName}' already contains a prefix. Which is not allowed.");
        }

        if (commandUniqueName.StartsWith(extensionUniqueName) || commandUniqueName.StartsWith("ArcTexel."))
        {
            return commandUniqueName;
        }

        return $"{extensionUniqueName}:{commandUniqueName}";
    }
}
