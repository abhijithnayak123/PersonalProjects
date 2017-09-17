using System;
using System.Collections.Generic;
using System.Linq;

namespace P3Net.Data
{
    /// <summary>Provides support for converting between types.</summary>
    public static class TypeConversion
    {
        /// <summary>Coerces a smaller numeric type to a larger type.</summary>
        /// <typeparam name="T">The larger type.</typeparam>
        /// <param name="value">The value to coerce.</param>
        /// <returns>The coerced value.</returns>
        /// <remarks>
        /// The standard type coercion rules used by most compilers are used to coerce the value.  The actual value is ignored and only
        /// the type rules are used unlike standard type conversion.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="value"/> is not a primitive type.</exception>
        /// <exception cref="InvalidCastException">The value's type cannot be coerced to the desired type.</exception>
        public static T Coerce<T> ( object value ) where T: struct
        {
            if (value ==  null)
                throw new ArgumentNullException("value");

            var desiredType = typeof(T);
            var valueType = value.GetType();

            if (desiredType == valueType)
                return (T)value;

            if (!IsNumericType(valueType))
                throw new ArgumentException("Value type is not a primitive.", "value");

            //Make sure the types are compatible first - doesn't seem to be any framework functionality to give us this information
            if (desiredType == typeof(decimal))
            {
                return (T)Convert.ChangeType(value, typeof(decimal));
            } else if (desiredType == typeof(double))
            {
                if (valueType != typeof(decimal))
                    return (T)Convert.ChangeType(value, typeof(double));
            } else if (desiredType == typeof(float))
            {
                if (!IsType(valueType, typeof(decimal), typeof(double)))
                    return (T)Convert.ChangeType(value, typeof(float));
            } else if (desiredType == typeof(ulong))
            {
                if (!IsType(valueType, typeof(decimal), typeof(double), typeof(float)))
                    return (T)Convert.ChangeType(value, typeof(ulong));
            } else if (desiredType == typeof(long))
            {
                if (!IsType(valueType, typeof(decimal), typeof(double), typeof(float), typeof(ulong)))
                    return (T)Convert.ChangeType(value, typeof(long));
            } else if (desiredType == typeof(uint))
            {
                if (IsType(valueType, typeof(int), typeof(ushort), typeof(short), typeof(byte), typeof(sbyte)))
                    return (T)Convert.ChangeType(value, typeof(uint));
            } else if (desiredType == typeof(int))
            {
                if (IsType(valueType, typeof(ushort), typeof(short), typeof(byte), typeof(sbyte)))
                    return (T)Convert.ChangeType(value, typeof(int));
            } else if (desiredType == typeof(ushort))
            {
                if (IsType(valueType, typeof(short), typeof(byte), typeof(sbyte)))
                    return (T)Convert.ChangeType(value, typeof(ushort));
            } else if (desiredType == typeof(short))
            {
                if (IsType(valueType, typeof(byte), typeof(sbyte)))
                    return (T)Convert.ChangeType(value, typeof(short));
            } else if (desiredType == typeof(byte))
            {
                if (valueType == typeof(sbyte))
                    return (T)Convert.ChangeType(value, typeof(byte));
            }
            ;

            throw new InvalidCastException(String.Format("Cannot coerce from '{0}' to '{1}'", valueType.Name, desiredType.Name));
        }

        #region Private Members

        private static bool IsNumericType ( Type type )
        {
            return IsType(type, typeof(decimal), typeof(double), typeof(float),
                               typeof(long), typeof(int), typeof(short), typeof(sbyte),
                               typeof(ulong), typeof(uint), typeof(ushort), typeof(byte));
        }

        private static bool IsType ( Type baseType, params Type[] possibleTypes )
        {
            return (from t in possibleTypes
                    where baseType == t
                    select t).Any();
        }
        #endregion
    }
}
