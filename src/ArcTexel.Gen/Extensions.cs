using System.Text;
using Microsoft.CodeAnalysis;

namespace ArcTexelGen;

internal static class Extensions
{
    public static bool AssignableTo(this INamedTypeSymbol? type, INamedTypeSymbol target)
    {
        while (true)
        {
            if (type == null)
            {
                return false;
            }

            if (type.Name == target.Name)
            {
                return true;
            }

            type = type.BaseType;
        }
    }
}
