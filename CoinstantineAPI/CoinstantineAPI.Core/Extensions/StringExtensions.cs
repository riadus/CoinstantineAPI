using System;

namespace CoinstantineAPI.Core.Extensions
{
    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string self)
        {
            return string.IsNullOrEmpty(self);
        }

        public static bool IsNotNull(this string self)
        {
            return !string.IsNullOrEmpty(self);
        }

		public static object ToEnum(this string val, Type targetType)
        {
            if (val.IsNullOrEmpty())
            {
                return null;
            }

            var nullableType = Nullable.GetUnderlyingType(targetType);

            if (nullableType != null)
            {
                targetType = nullableType;
            }

            var value = val.Replace("-", "").Replace(" ", "");
            var enumValue = Enum.Parse(targetType, value, true);
            return enumValue;
        }

        public static T ToEnum<T>(this string val) => (T)ToEnum(val, typeof(T));
    }
}
