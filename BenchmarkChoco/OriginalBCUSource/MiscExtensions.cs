using System;

namespace BenchmarkChoco
{
    public static class MiscExtensions
    {
        // Buggy with signed enums
        /*public static bool Contains(this Enum keys, Enum flag)
        {
            var keysVal = Convert.ToUInt64(keys);
            var flagVal = Convert.ToUInt64(flag);

            return (keysVal & flagVal) == flagVal;
        }*/

        /// <summary>
        ///     Check if this struct is equal to the default value for this type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        public static bool IsDefault<T>(this T value)
            where T : struct
        {
            var isDefault = value.Equals(default(T));

            return isDefault;
        }

        public static bool IsEmpty(this Guid obj)
        {
            return Guid.Empty.Equals(obj);
        }

        public static bool IsZeroOrNull(this Version obj)
        {
            return obj?.Equals(new Version(0, 0, 0, 0)) != false
                || obj.Equals(new Version(0, 0, 0)) || obj.Equals(new Version(0, 0));
        }
    }
}
