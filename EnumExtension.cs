using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Reflection;
using System.Monads;

namespace NextPms.Util.Extensions
{
    public static class EnumExtension
    {
        private static readonly ConcurrentDictionary<string, string> Cache = new ConcurrentDictionary<string, string>();

        public static string OfDescription<T>(this T value) where T : struct, IConvertible
        {
            var cacheKey = typeof (T).FullName + value.ToString();

            var result = Cache.GetOrAdd(cacheKey, key =>
            {
                var enumType = value.GetType();

                return
                    Enum.GetName(enumType, value)
                        .If(name => !string.IsNullOrEmpty(name))
                        .With(name => enumType.GetField(name))
                        .With(field =>
                             Attribute.GetCustomAttribute(field, typeof (DescriptionAttribute), false) as
                                    DescriptionAttribute)
                        .With(descriptionAttribute => descriptionAttribute.Description);
            });

            return result;
        }
    }
}
