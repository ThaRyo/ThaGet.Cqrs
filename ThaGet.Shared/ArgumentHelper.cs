using System;
using System.Collections.Generic;
using System.Linq;


namespace ThaGet.Shared
{
    public static class ArgumentHelper
    {
        public static bool IsEmpty(this int parameter) => (parameter == default);
        public static bool IsEmpty(this long parameter) => (parameter == default);
        public static bool IsEmpty(this double parameter) => (parameter == default);
        public static bool IsEmpty(this decimal parameter) => (parameter == default);
        public static bool IsEmpty(this Guid parameter) => (parameter == default);
        public static bool IsEmpty(this Guid? parameter) => (parameter == default);

        public static bool IsEmpty<T>(this IEnumerable<T> parameter) => (parameter == default || !parameter.Any());

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> parameter) => (parameter == null || IsEmpty(parameter));
        public static bool IsNullOrEmpty<T>(this T parameter) => (EqualityComparer<T>.Default.Equals(parameter, default));

        public static void ThrowIfNullOrEmpty(this string parameter, string parameterName, string message = null)
        {
            bool fn() => IsEmpty(parameter);
            ThrowIfNullOrEmpty(fn, parameter, parameterName, message);
        }

        public static void ThrowIfNullOrEmpty<T>(this IEnumerable<T> parameter, string parameterName, string message = null)
        {
            bool fn() => IsEmpty(parameter);
            ThrowIfNullOrEmpty(fn, parameter, parameterName, message);
        }

        public static void ThrowIfNull(this object parameter, string parameterName, string message = null)
        {
            static bool fn() => false;
            ThrowIfNullOrEmpty(fn, parameter, parameterName, message);
        }

        private static void ThrowIfNullOrEmpty<T>(Func<bool> condition, T parameter, string parameterName, string message)
        {
            if (parameter == null)
                throw new ArgumentNullException(parameterName);

            if (condition())
                throw new ArgumentException(parameterName, message);
        }

        public static void ThrowIfEmpty(this int parameter, string parameterName, string message = null)
        {
            bool fn() => IsEmpty(parameter);
            ThrowIfEmpty(fn, parameterName, message);
        }

        public static void ThrowIfEmpty(this long parameter, string parameterName, string message = null)
        {
            bool fn() => IsEmpty(parameter);
            ThrowIfEmpty(fn, parameterName, message);
        }

        public static void ThrowIfEmpty(this double parameter, string parameterName, string message = null)
        {
            bool fn() => IsEmpty(parameter);
            ThrowIfEmpty(fn, parameterName, message);
        }

        public static void ThrowIfEmpty(this decimal parameter, string parameterName, string message = null)
        {
            bool fn() => IsEmpty(parameter);
            ThrowIfEmpty(fn, parameterName, message);
        }

        public static void ThrowIfEmpty(this Guid parameter, string parameterName, string message = null)
        {
            bool fn() => IsEmpty(parameter);
            ThrowIfEmpty(fn, parameterName, message);
        }

        public static void ThrowIfEmpty(this Guid? parameter, string parameterName, string message = null)
        {
            bool fn() => IsEmpty(parameter);
            ThrowIfEmpty(fn, parameterName, message);
        }

        private static void ThrowIfEmpty(Func<bool> condition, string parameterName, string message)
        {
            if (condition())
                throw new ArgumentException(parameterName, message);
        }
    }
}
