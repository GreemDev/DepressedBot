using System;
using System.Reflection;

namespace DepressedBot.Extensions
{
    public static class ReflectionExtensions
    {
        public static bool HasAttribute<T>(this TypeInfo type) where T : Attribute
            => Attribute.GetCustomAttribute(type, typeof(T)) != null;

        public static bool HasAttribute<T>(this Type type) where T : Attribute
            => Attribute.GetCustomAttribute(type, typeof(T)) != null;
    }
}
