using System.Diagnostics.CodeAnalysis;
using Core.Exceptions;

namespace Core.Extensions;

public static class ObjectExtensions
{
    public static void ThrowNotFoundIfNull<T>([NotNull] this T? obj, object key) where T : class
    {
        if (obj == null)
        {
            throw NotFoundException.ForType<T>(key.ToString()!);
        }
    }

    public static void ThrowNotFoundIfNull<T>([NotNull] this T? obj) where T : class
    {
        if (obj == null)
        {
            throw new NotFoundException(typeof(T).Name);
        }
    }
}