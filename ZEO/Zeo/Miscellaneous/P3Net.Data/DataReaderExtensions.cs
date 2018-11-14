﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P3Net.Data
{
    /// <summary>Provides extension methods for <see cref="IDataRecord"/> implementations.</summary>
    public static class DataReaderExtensions
    {
        /// <summary>Determines if a specific field is in the result set.</summary>
        /// <param name="source">The source.</param>
        /// <param name="name">The name of the field to find.</param>
        /// <returns><see langword="true"/> if the field exists or <see langword="false"/> otherwise.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="name"/> is empty.</exception>
        public static bool FieldExists ( this IDataReader source, string name )
        {
            if (name == null)
                throw new ArgumentNullException("name");
            if (String.IsNullOrEmpty(name))
                throw new ArgumentException("Name cannot be empty.", "name");

            return (from i in GetNames(source)
                    where String.Compare(i, name, StringComparison.OrdinalIgnoreCase) == 0
                    select i).Any();
        }

        /// <summary>Gets the column value.</summary>
        /// <param name="source">The source.</param>
        /// <param name="name">The name of the column to retrieve.</param>
        /// <returns>The typed value.</returns>
        /// <exception cref="InvalidCastException">The column is not of the appropriate type.</exception>
        /// <exception cref="InvalidOperationException">The reader is closed.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="name"/> is not a valid field name.</exception>
        public static bool GetBoolean ( this IDataRecord source, string name )
        {
            int ordinal = VerifyGetOrdinal(source, name);

            return source.GetBoolean(ordinal);
        }

        /// <summary>Gets the column value or the default value if the column is not set.</summary>
        /// <param name="source">The source</param>
        /// <param name="ordinal">The zero-based ordinal.</param>
        /// <returns>The typed value or the default type value if the column was not set.</returns>
        /// <exception cref="InvalidCastException">The column is not of the appropriate type.</exception>
        /// <exception cref="InvalidOperationException">The reader is closed.</exception>
        /// <exception cref="IndexOutOfRangeException">
        /// <paramref name="ordinal"/> is less than zero.
        /// <para>-or-</para>
        /// <paramref name="ordinal"/> is too big for the reader.
        /// </exception>
        public static bool GetBooleanOrDefault ( this IDataRecord source, int ordinal )
        {
            return GetBooleanOrDefault(source, ordinal, false);
        }

        /// <summary>Gets the column value or the default value if the column is not set.</summary>
        /// <param name="source">The source</param>
        /// <param name="ordinal">The zero-based ordinal.</param>        
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The typed value or the default type value if the column was not set.</returns>
        /// <exception cref="InvalidCastException">The column is not of the appropriate type.</exception>
        /// <exception cref="InvalidOperationException">The reader is closed.</exception>
        /// <exception cref="IndexOutOfRangeException">
        /// <paramref name="ordinal"/> is less than zero.
        /// <para>-or-</para>
        /// <paramref name="ordinal"/> is too big for the reader.
        /// </exception>
        public static bool GetBooleanOrDefault ( this IDataRecord source, int ordinal, bool defaultValue )
        {
            if (!source.IsDBNull(ordinal))
                return source.GetBoolean(ordinal);

            return defaultValue;
        }

        /// <summary>Gets the column value or the default value if the column is not set.</summary>
        /// <param name="source">The source</param>
        /// <param name="name">The name of the column to retrieve.</param>
        /// <returns>The typed value or the default type value if the column was not set.</returns>
        /// <exception cref="InvalidCastException">The column is not of the appropriate type.</exception>
        /// <exception cref="InvalidOperationException">The reader is closed.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="name"/> is not a valid field name.</exception>
        public static bool GetBooleanOrDefault ( this IDataRecord source, string name )
        {
            return GetBooleanOrDefault(source, name, false);
        }

        /// <summary>Gets the column value or the default value if the column is not set.</summary>
        /// <param name="source">The source</param>
        /// <param name="name">The name of the column to retrieve.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The typed value or the default type value if the column was not set.</returns>
        /// <exception cref="InvalidCastException">The column is not of the appropriate type.</exception>
        /// <exception cref="InvalidOperationException">The reader is closed.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="name"/> is not a valid field name.</exception>
        public static bool GetBooleanOrDefault ( this IDataRecord source, string name, bool defaultValue )
        {
            int ordinal = VerifyGetOrdinal(source, name);

            return GetBooleanOrDefault(source, ordinal, defaultValue);
        }

        /// <summary>Gets the column value.</summary>
        /// <param name="source">The source</param>
        /// <param name="name">The name of the column to retrieve.</param>
        /// <returns>The typed value.</returns>
        /// <exception cref="InvalidCastException">The column is not of the appropriate type.</exception>
        /// <exception cref="InvalidOperationException">The reader is closed.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="name"/> is not a valid field name.</exception>
        /// <remarks>
        /// To be consistent with the framework the method will throw an <see cref="InvalidCastException"/> if the column type does not
        /// exactly match the method type.
        /// </remarks>
        /// <example>
        /// Refer to <see cref="DataReaderExtensions"/> for an example of how to use this method.
        /// </example>
        public static byte GetByte ( this IDataRecord source, string name )
        {
            int ordinal = VerifyGetOrdinal(source, name);

            return source.GetByte(ordinal);
        }

        /// <summary>Gets the column value or the default value if the column is not set.</summary>
        /// <param name="source">The source</param>
        /// <param name="ordinal">The zero-based ordinal.</param>
        /// <returns>The typed value or the default type value if the column was not set.</returns>
        /// <exception cref="InvalidCastException">The column is not of the appropriate type.</exception>
        /// <exception cref="InvalidOperationException">The reader is closed.</exception>
        /// <exception cref="IndexOutOfRangeException">
        /// <paramref name="ordinal"/> is less than zero.
        /// <para>-or-</para>
        /// <paramref name="ordinal"/> is too big for the reader.
        /// </exception>
        public static byte GetByteOrDefault ( this IDataRecord source, int ordinal )
        {
            return GetByteOrDefault(source, ordinal, 0);
        }

        /// <summary>Gets the column value or the default value if the column is not set.</summary>
        /// <param name="source">The source</param>
        /// <param name="ordinal">The zero-based ordinal.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The typed value or the default type value if the column was not set.</returns>
        /// <exception cref="InvalidCastException">The column is not of the appropriate type.</exception>
        /// <exception cref="InvalidOperationException">The reader is closed.</exception>
        /// <exception cref="IndexOutOfRangeException">
        /// <paramref name="ordinal"/> is less than zero.
        /// <para>-or-</para>
        /// <paramref name="ordinal"/> is too big for the reader.
        /// </exception>
        public static byte GetByteOrDefault ( this IDataRecord source, int ordinal, byte defaultValue )
        {
            if (!source.IsDBNull(ordinal))
                return source.GetByte(ordinal);

            return defaultValue;
        }

        /// <summary>Gets the column value or the default value if the column is not set.</summary>
        /// <param name="source">The source</param>
        /// <param name="name">The name of the column to retrieve.</param>
        /// <returns>The typed value or the default type value if the column was not set.</returns>
        /// <exception cref="InvalidCastException">The column is not of the appropriate type.</exception>
        /// <exception cref="InvalidOperationException">The reader is closed.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="name"/> is not a valid field name.</exception>
        public static byte GetByteOrDefault ( this IDataRecord source, string name )
        {
            return GetByteOrDefault(source, name, 0);
        }

        /// <summary>Gets the column value or the default value if the column is not set.</summary>
        /// <param name="source">The source</param>
        /// <param name="name">The name of the column to retrieve.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The typed value or the default type value if the column was not set.</returns>
        /// <exception cref="InvalidCastException">The column is not of the appropriate type.</exception>
        /// <exception cref="InvalidOperationException">The reader is closed.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="name"/> is not a valid field name.</exception>
        public static byte GetByteOrDefault ( this IDataRecord source, string name, byte defaultValue )
        {
            int ordinal = VerifyGetOrdinal(source, name);

            return GetByteOrDefault(source, ordinal, defaultValue);
        }

        /// <summary>Gets the value of the specified column as an array of bytes.</summary>
        /// <param name="source">The source</param>
        /// <param name="name">The name of the column to retrieve.</param>
        /// <param name="dataOffset">The offset within the column to retrieve the data.</param>
        /// <param name="buffer">The buffer to store the data into.</param>
        /// <param name="bufferOffset">The offset within the buffer to start storing data.</param>
        /// <param name="length">The number of byte to copy.</param>
        /// <returns>The size of the data in bytes.</returns>
        /// <exception cref="InvalidCastException">The column is not of the appropriate type.</exception>
        /// <exception cref="InvalidOperationException">The reader is closed.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="name"/> is not a valid field name.</exception>
        public static long GetBytes ( this IDataRecord source, string name, long dataOffset, byte[] buffer, int bufferOffset, int length )
        {
            int ordinal = VerifyGetOrdinal(source, name);

            return source.GetBytes(ordinal, dataOffset, buffer, bufferOffset, length);
        }

        /// <summary>Gets the column value.</summary>
        /// <param name="source">The source</param>
        /// <param name="name">The name of the column to retrieve.</param>
        /// <returns>The typed value.</returns>
        /// <exception cref="InvalidCastException">The column is not of the appropriate type.</exception>
        /// <exception cref="InvalidOperationException">The reader is closed.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="name"/> is not a valid field name.</exception>
        public static char GetChar ( this IDataRecord source, string name )
        {
            int ordinal = VerifyGetOrdinal(source, name);

            return source.GetChar(ordinal);
        }

        /// <summary>Gets the column value or the default value if the column is not set.</summary>
        /// <param name="source">The source</param>
        /// <param name="ordinal">The zero-based ordinal.</param>
        /// <returns>The typed value or the default type value if the column was not set.</returns>
        /// <exception cref="InvalidCastException">The column is not of the appropriate type.</exception>
        /// <exception cref="InvalidOperationException">The reader is closed.</exception>
        /// <exception cref="IndexOutOfRangeException">
        /// <paramref name="ordinal"/> is less than zero.
        /// <para>-or-</para>
        /// <paramref name="ordinal"/> is too big for the reader.
        /// </exception>
        public static char GetCharOrDefault ( this IDataRecord source, int ordinal )
        {
            return GetCharOrDefault(source, ordinal, default(char));
        }

        /// <summary>Gets the column value or the default value if the column is not set.</summary>
        /// <param name="source">The source</param>
        /// <param name="ordinal">The zero-based ordinal.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The typed value or the default type value if the column was not set.</returns>
        /// <exception cref="InvalidCastException">The column is not of the appropriate type.</exception>
        /// <exception cref="InvalidOperationException">The reader is closed.</exception>
        /// <exception cref="IndexOutOfRangeException">
        /// <paramref name="ordinal"/> is less than zero.
        /// <para>-or-</para>
        /// <paramref name="ordinal"/> is too big for the reader.
        /// </exception>
        public static char GetCharOrDefault ( this IDataRecord source, int ordinal, char defaultValue )
        {
            if (!source.IsDBNull(ordinal))
                return source.GetChar(ordinal);

            return defaultValue;
        }

        /// <summary>Gets the column value or the default value if the column is not set.</summary>
        /// <param name="source">The source</param>
        /// <param name="name">The name of the column to retrieve.</param>
        /// <returns>The typed value or the default type value if the column was not set.</returns>
        /// <exception cref="InvalidCastException">The column is not of the appropriate type.</exception>
        /// <exception cref="InvalidOperationException">The reader is closed.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="name"/> is not a valid field name.</exception>
        public static char GetCharOrDefault ( this IDataRecord source, string name )
        {
            return GetCharOrDefault(source, name, default(char));
        }

        /// <summary>Gets the column value or the default value if the column is not set.</summary>
        /// <param name="source">The source</param>
        /// <param name="name">The name of the column to retrieve.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The typed value or the default type value if the column was not set.</returns>
        /// <exception cref="InvalidCastException">The column is not of the appropriate type.</exception>
        /// <exception cref="InvalidOperationException">The reader is closed.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="name"/> is not a valid field name.</exception>
        public static char GetCharOrDefault ( this IDataRecord source, string name, char defaultValue )
        {
            int ordinal = VerifyGetOrdinal(source, name);

            return GetCharOrDefault(source, ordinal, defaultValue);
        }

        /// <summary>Gets the value of the specified column as an array of characters.</summary>
        /// <param name="source">The source</param>
        /// <param name="name">The name of the column to retrieve.</param>
        /// <param name="fieldOffset">The offset within the column to retrieve the data.</param>
        /// <param name="buffer">The buffer to store the data into.</param>
        /// <param name="bufferOffset">The offset within the buffer to start storing data.</param>
        /// <param name="length">The number of characters to copy.</param>
        /// <returns>The size of the value in characters.</returns>
        /// <exception cref="InvalidCastException">The column is not of the appropriate type.</exception>
        /// <exception cref="InvalidOperationException">The reader is closed.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="name"/> is not a valid field name.</exception>		
        public static long GetChars ( this IDataRecord source, string name, long fieldOffset, char[] buffer, int bufferOffset, int length )
        {
            int ordinal = VerifyGetOrdinal(source, name);

            return source.GetChars(ordinal, fieldOffset, buffer, bufferOffset, length);
        }

        /// <summary>Gets the data type of the specified column.</summary>
        /// <param name="source">The source</param>
        /// <param name="name">The name of the column to retrieve.</param>
        /// <returns>The type name of the column.</returns>
        /// <exception cref="InvalidOperationException">The reader is closed.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="name"/> is not a valid field name.</exception>
        public static string GetDataTypeName ( this IDataRecord source, string name )
        {
            int ordinal = VerifyGetOrdinal(source, name);

            return source.GetDataTypeName(ordinal);
        }

        /// <summary>Gets the column value.</summary>
        /// <param name="source">The source</param>
        /// <param name="name">The name of the column to retrieve.</param>
        /// <returns>The typed value.</returns>
        /// <exception cref="InvalidCastException">The column is not of the appropriate type.</exception>
        /// <exception cref="InvalidOperationException">The reader is closed.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="name"/> is not a valid field name.</exception>
        public static DateTime GetDateTime ( this IDataRecord source, string name )
        {
            int ordinal = VerifyGetOrdinal(source, name);

            return source.GetDateTime(ordinal);
        }

        /// <summary>Gets the column value or the default value if the column is not set.</summary>
        /// <param name="source">The source</param>
        /// <param name="ordinal">The zero-based ordinal.</param>
        /// <returns>The typed value or the default type value if the column was not set.</returns>
        /// <exception cref="InvalidCastException">The column is not of the appropriate type.</exception>
        /// <exception cref="InvalidOperationException">The reader is closed.</exception>
        /// <exception cref="IndexOutOfRangeException">
        /// <paramref name="ordinal"/> is less than zero.
        /// <para>-or-</para>
        /// <paramref name="ordinal"/> is too big for the reader.
        /// </exception>
        public static DateTime GetDateTimeOrDefault ( this IDataRecord source, int ordinal )
        {
            return GetDateTimeOrDefault(source, ordinal, DateTime.MinValue);
        }

        /// <summary>Gets the column value or the default value if the column is not set.</summary>
        /// <param name="source">The source</param>
        /// <param name="ordinal">The zero-based ordinal.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The typed value or the default type value if the column was not set.</returns>
        /// <exception cref="InvalidCastException">The column is not of the appropriate type.</exception>
        /// <exception cref="InvalidOperationException">The reader is closed.</exception>
        /// <exception cref="IndexOutOfRangeException">
        /// <paramref name="ordinal"/> is less than zero.
        /// <para>-or-</para>
        /// <paramref name="ordinal"/> is too big for the reader.
        /// </exception>
        public static DateTime GetDateTimeOrDefault ( this IDataRecord source, int ordinal, DateTime defaultValue )
        {
            if (!source.IsDBNull(ordinal))
                return source.GetDateTime(ordinal);

            return defaultValue;
        }

        /// <summary>Gets the column value or the default value if the column is not set.</summary>
        /// <param name="source">The source</param>
        /// <param name="name">The name of the column to retrieve.</param>
        /// <returns>The typed value or the default type value if the column was not set.</returns>
        /// <exception cref="InvalidCastException">The column is not of the appropriate type.</exception>
        /// <exception cref="InvalidOperationException">The reader is closed.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="name"/> is not a valid field name.</exception>
        public static DateTime GetDateTimeOrDefault ( this IDataRecord source, string name )
        {
            return GetDateTimeOrDefault(source, name, DateTime.MinValue);
        }

        /// <summary>Gets the column value or the default value if the column is not set.</summary>
        /// <param name="source">The source</param>
        /// <param name="name">The name of the column to retrieve.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The typed value or the default type value if the column was not set.</returns>
        /// <exception cref="InvalidCastException">The column is not of the appropriate type.</exception>
        /// <exception cref="InvalidOperationException">The reader is closed.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="name"/> is not a valid field name.</exception>
        public static DateTime GetDateTimeOrDefault ( this IDataRecord source, string name, DateTime defaultValue )
        {
            int ordinal = VerifyGetOrdinal(source, name);

            return GetDateTimeOrDefault(source, ordinal, defaultValue);
        }

        /// <summary>Gets the column value.</summary>
        /// <param name="source">The source</param>
        /// <param name="name">The name of the column to retrieve.</param>
        /// <returns>The typed value.</returns>
        /// <exception cref="InvalidCastException">The column is not of the appropriate type.</exception>
        /// <exception cref="InvalidOperationException">The reader is closed.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="name"/> is not a valid field name.</exception>
        /// <remarks>
        /// To be consistent with the framework the method will throw an <see cref="InvalidCastException"/> if the column type does not
        /// exactly match the method type.
        /// </remarks>
        public static decimal GetDecimal ( this IDataRecord source, string name )
        {
            int ordinal = VerifyGetOrdinal(source, name);

            return source.GetDecimal(ordinal);
        }

        /// <summary>Gets the column value or the default value if the column is not set.</summary>
        /// <param name="source">The source</param>
        /// <param name="ordinal">The zero-based ordinal.</param>
        /// <returns>The typed value or the default type value if the column was not set.</returns>
        /// <exception cref="InvalidCastException">The column is not of the appropriate type.</exception>
        /// <exception cref="InvalidOperationException">The reader is closed.</exception>
        /// <exception cref="IndexOutOfRangeException">
        /// <paramref name="ordinal"/> is less than zero.
        /// <para>-or-</para>
        /// <paramref name="ordinal"/> is too big for the reader.
        /// </exception>
        /// <remarks>
        /// Unlike the non-default method, smaller numeric types will be coerced as needed.  
        /// </remarks>
        public static decimal GetDecimalOrDefault ( this IDataRecord source, int ordinal )
        {
            return GetDecimalOrDefault(source, ordinal, 0);
        }

        /// <summary>Gets the column value or the default value if the column is not set.</summary>
        /// <param name="source">The source</param>
        /// <param name="ordinal">The zero-based ordinal.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The typed value or the default type value if the column was not set.</returns>
        /// <exception cref="InvalidCastException">The column is not of the appropriate type.</exception>
        /// <exception cref="InvalidOperationException">The reader is closed.</exception>
        /// <exception cref="IndexOutOfRangeException">
        /// <paramref name="ordinal"/> is less than zero.
        /// <para>-or-</para>
        /// <paramref name="ordinal"/> is too big for the reader.
        /// </exception>
        /// <remarks>
        /// Unlike the non-default method, smaller numeric types will be coerced as needed.  
        /// </remarks>
        public static decimal GetDecimalOrDefault ( this IDataRecord source, int ordinal, decimal defaultValue )
        {
            if (!source.IsDBNull(ordinal))
                return Coerce<decimal>(source, ordinal);

            return defaultValue;
        }

        /// <summary>Gets the column value or the default value if the column is not set.</summary>
        /// <param name="source">The source</param>
        /// <param name="name">The name of the column to retrieve.</param>
        /// <returns>The typed value or the default type value if the column was not set.</returns>
        /// <exception cref="InvalidCastException">The column is not of the appropriate type.</exception>
        /// <exception cref="InvalidOperationException">The reader is closed.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="name"/> is not a valid field name.</exception>
        /// <remarks>
        /// Unlike the non-default method, smaller numeric types will be coerced as needed.  
        /// </remarks>
        public static decimal GetDecimalOrDefault ( this IDataRecord source, string name )
        {
            return GetDecimalOrDefault(source, name, 0);
        }

        /// <summary>Gets the column value or the default value if the column is not set.</summary>
        /// <param name="source">The source</param>
        /// <param name="name">The name of the column to retrieve.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The typed value or the default type value if the column was not set.</returns>
        /// <exception cref="InvalidCastException">The column is not of the appropriate type.</exception>
        /// <exception cref="InvalidOperationException">The reader is closed.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="name"/> is not a valid field name.</exception>
        /// <remarks>
        /// Unlike the non-default method, smaller numeric types will be coerced as needed.  
        /// </remarks>
        public static decimal GetDecimalOrDefault ( this IDataRecord source, string name, decimal defaultValue )
        {
            int ordinal = VerifyGetOrdinal(source, name);

            return GetDecimalOrDefault(source, ordinal, defaultValue);
        }

        /// <summary>Gets the column value.</summary>
        /// <param name="source">The source</param>
        /// <param name="name">The name of the column to retrieve.</param>
        /// <returns>The typed value.</returns>
        /// <exception cref="InvalidCastException">The column is not of the appropriate type.</exception>
        /// <exception cref="InvalidOperationException">The reader is closed.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="name"/> is not a valid field name.</exception>
        /// <remarks>
        /// To be consistent with the framework the method will throw an <see cref="InvalidCastException"/> if the column type does not
        /// exactly match the method type.
        /// </remarks>
        public static double GetDouble ( this IDataRecord source, string name )
        {
            int ordinal = VerifyGetOrdinal(source, name);

            return source.GetDouble(ordinal);
        }

        /// <summary>Gets the column value or the default value if the column is not set.</summary>
        /// <param name="source">The source</param>
        /// <param name="ordinal">The zero-based ordinal.</param>
        /// <returns>The typed value or the default type value if the column was not set.</returns>
        /// <exception cref="InvalidCastException">The column is not of the appropriate type.</exception>
        /// <exception cref="InvalidOperationException">The reader is closed.</exception>
        /// <exception cref="IndexOutOfRangeException">
        /// <paramref name="ordinal"/> is less than zero.
        /// <para>-or-</para>
        /// <paramref name="ordinal"/> is too big for the reader.
        /// </exception>
        /// <remarks>
        /// Unlike the non-default method, smaller numeric types will be coerced as needed.  
        /// </remarks>
        public static double GetDoubleOrDefault ( this IDataRecord source, int ordinal )
        {
            return GetDoubleOrDefault(source, ordinal, 0);
        }

        /// <summary>Gets the column value or the default value if the column is not set.</summary>
        /// <param name="source">The source</param>
        /// <param name="ordinal">The zero-based ordinal.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The typed value or the default type value if the column was not set.</returns>
        /// <exception cref="InvalidCastException">The column is not of the appropriate type.</exception>
        /// <exception cref="InvalidOperationException">The reader is closed.</exception>
        /// <exception cref="IndexOutOfRangeException">
        /// <paramref name="ordinal"/> is less than zero.
        /// <para>-or-</para>
        /// <paramref name="ordinal"/> is too big for the reader.
        /// </exception>
        /// <remarks>
        /// Unlike the non-default method, smaller numeric types will be coerced as needed.  
        /// </remarks>
        public static double GetDoubleOrDefault ( this IDataRecord source, int ordinal, double defaultValue )
        {
            if (!source.IsDBNull(ordinal))
                return Coerce<double>(source, ordinal);

            return defaultValue;
        }

        /// <summary>Gets the column value or the default value if the column is not set.</summary>
        /// <param name="source">The source</param>
        /// <param name="name">The name of the column to retrieve.</param>
        /// <returns>The typed value or the default type value if the column was not set.</returns>
        /// <exception cref="InvalidCastException">The column is not of the appropriate type.</exception>
        /// <exception cref="InvalidOperationException">The reader is closed.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="name"/> is not a valid field name.</exception>
        /// <remarks>
        /// Unlike the non-default method, smaller numeric types will be coerced as needed.  
        /// </remarks>
        public static double GetDoubleOrDefault ( this IDataRecord source, string name )
        {
            return GetDoubleOrDefault(source, name, 0);
        }

        /// <summary>Gets the column value or the default value if the column is not set.</summary>
        /// <param name="source">The source</param>
        /// <param name="name">The name of the column to retrieve.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The typed value or the default type value if the column was not set.</returns>
        /// <exception cref="InvalidCastException">The column is not of the appropriate type.</exception>
        /// <exception cref="InvalidOperationException">The reader is closed.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="name"/> is not a valid field name.</exception>
        /// <remarks>
        /// Unlike the non-default method, smaller numeric types will be coerced as needed.  
        /// </remarks>
        public static double GetDoubleOrDefault ( this IDataRecord source, string name, double defaultValue )
        {
            int ordinal = VerifyGetOrdinal(source, name);

            return GetDoubleOrDefault(source, ordinal, defaultValue);
        }

        /// <summary>Gets the type of the specified column.</summary>
        /// <param name="source">The source</param>
        /// <param name="name">The name of the column to retrieve.</param>
        /// <returns>The type of the column.</returns>
        /// <exception cref="InvalidOperationException">The reader is closed.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="name"/> is not a valid field name.</exception>
        public static Type GetFieldType ( this IDataRecord source, string name )
        {
            int ordinal = VerifyGetOrdinal(source, name);

            return source.GetFieldType(ordinal);
        }

        /// <summary>Gets the column value.</summary>
        /// <param name="source">The source</param>
        /// <param name="ordinal">The zero-based ordinal.</param>
        /// <returns>The typed value.</returns>
        /// <exception cref="InvalidCastException">The column is not of the appropriate type.</exception>
        /// <exception cref="InvalidOperationException">The reader is closed.</exception>
        /// <exception cref="IndexOutOfRangeException">
        /// <paramref name="ordinal"/> is less than zero.
        /// <para>-or-</para>
        /// <paramref name="ordinal"/> is too big for the reader.        
        /// </exception>
        /// <remarks>
        /// To be consistent with the framework the method will throw an <see cref="InvalidCastException"/> if the column type does not
        /// exactly match the method type.
        /// </remarks>
        public static float GetSingle ( this IDataRecord source, int ordinal )
        {
            return source.GetFloat(ordinal);
        }

        /// <summary>Gets the column value.</summary>
        /// <param name="source">The source</param>
        /// <param name="name">The name of the column to retrieve.</param>
        /// <returns>The typed value.</returns>
        /// <exception cref="InvalidCastException">The column is not of the appropriate type.</exception>
        /// <exception cref="InvalidOperationException">The reader is closed.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="name"/> is not a valid field name.</exception>
        /// <remarks>
        /// To be consistent with the framework the method will throw an <see cref="InvalidCastException"/> if the column type does not
        /// exactly match the method type.
        /// </remarks>
        public static float GetSingle ( this IDataRecord source, string name )
        {
            int ordinal = VerifyGetOrdinal(source, name);

            return source.GetSingle(ordinal);
        }

        /// <summary>Gets the column value.</summary>
        /// <param name="source">The source</param>
        /// <param name="name">The name of the column to retrieve.</param>
        /// <returns>The typed value.</returns>
        /// <exception cref="InvalidCastException">The column is not of the appropriate type.</exception>
        /// <exception cref="InvalidOperationException">The reader is closed.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="name"/> is not a valid field name.</exception>
        public static Guid GetGuid ( this IDataRecord source, string name )
        {
            int ordinal = VerifyGetOrdinal(source, name);

            return source.GetGuid(ordinal);
        }

        /// <summary>Gets the column value or the default value if the column is not set.</summary>
        /// <param name="source">The source</param>
        /// <param name="ordinal">The zero-based ordinal.</param>
        /// <returns>The typed value or the default type value if the column was not set.</returns>
        /// <exception cref="InvalidCastException">The column is not of the appropriate type.</exception>
        /// <exception cref="InvalidOperationException">The reader is closed.</exception>
        /// <exception cref="IndexOutOfRangeException">
        /// <paramref name="ordinal"/> is less than zero.
        /// <para>-or-</para>
        /// <paramref name="ordinal"/> is too big for the reader.
        /// </exception>
        public static Guid GetGuidOrDefault ( this IDataRecord source, int ordinal )
        {
            return GetGuidOrDefault(source, ordinal, Guid.Empty);
        }

        /// <summary>Gets the column value or the default value if the column is not set.</summary>
        /// <param name="source">The source</param>
        /// <param name="ordinal">The zero-based ordinal.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The typed value or the default type value if the column was not set.</returns>
        /// <exception cref="InvalidCastException">The column is not of the appropriate type.</exception>
        /// <exception cref="InvalidOperationException">The reader is closed.</exception>
        /// <exception cref="IndexOutOfRangeException">
        /// <paramref name="ordinal"/> is less than zero.
        /// <para>-or-</para>
        /// <paramref name="ordinal"/> is too big for the reader.
        /// </exception>
        public static Guid GetGuidOrDefault ( this IDataRecord source, int ordinal, Guid defaultValue )
        {
            if (!source.IsDBNull(ordinal))
                return source.GetGuid(ordinal);

            return defaultValue;
        }

        /// <summary>Gets the column value or the default value if the column is not set.</summary>
        /// <param name="source">The source</param>
        /// <param name="name">The name of the column to retrieve.</param>
        /// <returns>The typed value or the default type value if the column was not set.</returns>
        /// <exception cref="InvalidCastException">The column is not of the appropriate type.</exception>
        /// <exception cref="InvalidOperationException">The reader is closed.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="name"/> is not a valid field name.</exception>
        public static Guid GetGuidOrDefault ( this IDataRecord source, string name )
        {
            return GetGuidOrDefault(source, name, Guid.Empty);
        }

        /// <summary>Gets the column value or the default value if the column is not set.</summary>
        /// <param name="source">The source</param>
        /// <param name="name">The name of the column to retrieve.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The typed value or the default type value if the column was not set.</returns>
        /// <exception cref="InvalidCastException">The column is not of the appropriate type.</exception>
        /// <exception cref="InvalidOperationException">The reader is closed.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="name"/> is not a valid field name.</exception>
        public static Guid GetGuidOrDefault ( this IDataRecord source, string name, Guid defaultValue )
        {
            int ordinal = VerifyGetOrdinal(source, name);

            return GetGuidOrDefault(source, ordinal, defaultValue);
        }

        /// <summary>Gets the column value.</summary>
        /// <param name="source">The source</param>
        /// <param name="name">The name of the column to retrieve.</param>
        /// <returns>The typed value.</returns>
        /// <exception cref="InvalidCastException">The column is not of the appropriate type.</exception>
        /// <exception cref="InvalidOperationException">The reader is closed.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="name"/> is not a valid field name.</exception>
        /// <remarks>
        /// To be consistent with the framework the method will throw an <see cref="InvalidCastException"/> if the column type does not
        /// exactly match the method type.
        /// </remarks>
        public static short GetInt16 ( this IDataRecord source, string name )
        {
            int ordinal = VerifyGetOrdinal(source, name);

            return source.GetInt16(ordinal);
        }

        /// <summary>Gets the column value or the default value if the column is not set.</summary>
        /// <param name="source">The source</param>
        /// <param name="ordinal">The zero-based ordinal.</param>
        /// <returns>The typed value or the default type value if the column was not set.</returns>
        /// <exception cref="InvalidCastException">The column is not of the appropriate type.</exception>
        /// <exception cref="InvalidOperationException">The reader is closed.</exception>
        /// <exception cref="IndexOutOfRangeException">
        /// <paramref name="ordinal"/> is less than zero.
        /// <para>-or-</para>
        /// <paramref name="ordinal"/> is too big for the reader.
        /// </exception>
        /// <remarks>
        /// Unlike the non-default method, smaller numeric types will be coerced as needed.  
        /// </remarks>
        public static short GetInt16OrDefault ( this IDataRecord source, int ordinal )
        {
            return GetInt16OrDefault(source, ordinal, 0);
        }

        /// <summary>Gets the column value or the default value if the column is not set.</summary>
        /// <param name="source">The source</param>
        /// <param name="ordinal">The zero-based ordinal.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The typed value or the default type value if the column was not set.</returns>
        /// <exception cref="InvalidCastException">The column is not of the appropriate type.</exception>
        /// <exception cref="InvalidOperationException">The reader is closed.</exception>
        /// <exception cref="IndexOutOfRangeException">
        /// <paramref name="ordinal"/> is less than zero.
        /// <para>-or-</para>
        /// <paramref name="ordinal"/> is too big for the reader.
        /// </exception>
        /// <remarks>
        /// Unlike the non-default method, smaller numeric types will be coerced as needed.  
        /// </remarks>
        public static short GetInt16OrDefault ( this IDataRecord source, int ordinal, short defaultValue )
        {
            if (!source.IsDBNull(ordinal))
                return Coerce<short>(source, ordinal);

            return defaultValue;
        }

        /// <summary>Gets the column value or the default value if the column is not set.</summary>
        /// <param name="source">The source</param>
        /// <param name="name">The name of the column to retrieve.</param>
        /// <returns>The typed value or the default type value if the column was not set.</returns>
        /// <exception cref="InvalidCastException">The column is not of the appropriate type.</exception>
        /// <exception cref="InvalidOperationException">The reader is closed.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="name"/> is not a valid field name.</exception>
        /// <remarks>
        /// Unlike the non-default method, smaller numeric types will be coerced as needed.  
        /// </remarks>
        public static short GetInt16OrDefault ( this IDataRecord source, string name )
        {
            return GetInt16OrDefault(source, name, 0);
        }

        /// <summary>Gets the column value or the default value if the column is not set.</summary>
        /// <param name="source">The source</param>
        /// <param name="name">The name of the column to retrieve.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The typed value or the default type value if the column was not set.</returns>
        /// <exception cref="InvalidCastException">The column is not of the appropriate type.</exception>
        /// <exception cref="InvalidOperationException">The reader is closed.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="name"/> is not a valid field name.</exception>
        /// <remarks>
        /// Unlike the non-default method, smaller numeric types will be coerced as needed.  
        /// </remarks>
        public static short GetInt16OrDefault ( this IDataRecord source, string name, short defaultValue )
        {
            int ordinal = VerifyGetOrdinal(source, name);

            return GetInt16OrDefault(source, ordinal, defaultValue);
        }

        /// <summary>Gets the column value.</summary>
        /// <param name="source">The source</param>
        /// <param name="name">The name of the column to retrieve.</param>
        /// <returns>The typed value.</returns>
        /// <exception cref="InvalidCastException">The column is not of the appropriate type.</exception>
        /// <exception cref="InvalidOperationException">The reader is closed.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="name"/> is not a valid field name.</exception>
        /// <remarks>
        /// To be consistent with the framework the method will throw an <see cref="InvalidCastException"/> if the column type does not
        /// exactly match the method type.
        /// </remarks>
        public static int GetInt32 ( this IDataRecord source, string name )
        {
            int ordinal = VerifyGetOrdinal(source, name);

            return source.GetInt32(ordinal);
        }

        /// <summary>Gets the column value or the default value if the column is not set.</summary>
        /// <param name="source">The source</param>
        /// <param name="ordinal">The zero-based ordinal.</param>
        /// <returns>The typed value or the default type value if the column was not set.</returns>
        /// <exception cref="InvalidCastException">The column is not of the appropriate type.</exception>
        /// <exception cref="InvalidOperationException">The reader is closed.</exception>
        /// <exception cref="IndexOutOfRangeException">
        /// <paramref name="ordinal"/> is less than zero.
        /// <para>-or-</para>
        /// <paramref name="ordinal"/> is too big for the reader.
        /// </exception>
        /// <remarks>
        /// Unlike the non-default method, smaller numeric types will be coerced as needed.  
        /// </remarks>
        public static int GetInt32OrDefault ( this IDataRecord source, int ordinal )
        {
            return GetInt32OrDefault(source, ordinal, 0);
        }

        /// <summary>Gets the column value or the default value if the column is not set.</summary>
        /// <param name="source">The source</param>
        /// <param name="ordinal">The zero-based ordinal.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The typed value or the default type value if the column was not set.</returns>
        /// <exception cref="InvalidCastException">The column is not of the appropriate type.</exception>
        /// <exception cref="InvalidOperationException">The reader is closed.</exception>
        /// <exception cref="IndexOutOfRangeException">
        /// <paramref name="ordinal"/> is less than zero.
        /// <para>-or-</para>
        /// <paramref name="ordinal"/> is too big for the reader.
        /// </exception>
        /// <remarks>
        /// Unlike the non-default method, smaller numeric types will be coerced as needed.  
        /// </remarks>
        public static int GetInt32OrDefault ( this IDataRecord source, int ordinal, int defaultValue )
        {
            if (!source.IsDBNull(ordinal))
                return Coerce<int>(source, ordinal);

            return defaultValue;
        }

        /// <summary>Gets the column value or the default value if the column is not set.</summary>
        /// <param name="source">The source</param>
        /// <param name="name">The name of the column to retrieve.</param>
        /// <returns>The typed value or the default type value if the column was not set.</returns>
        /// <exception cref="InvalidCastException">The column is not of the appropriate type.</exception>
        /// <exception cref="InvalidOperationException">The reader is closed.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="name"/> is not a valid field name.</exception>
        /// <remarks>
        /// Unlike the non-default method, smaller numeric types will be coerced as needed.  
        /// </remarks>
        public static int GetInt32OrDefault ( this IDataRecord source, string name )
        {
            return GetInt32OrDefault(source, name, 0);
        }

        /// <summary>Gets the column value or the default value if the column is not set.</summary>
        /// <param name="source">The source</param>
        /// <param name="name">The name of the column to retrieve.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The typed value or the default type value if the column was not set.</returns>
        /// <exception cref="InvalidCastException">The column is not of the appropriate type.</exception>
        /// <exception cref="InvalidOperationException">The reader is closed.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="name"/> is not a valid field name.</exception>
        /// <remarks>
        /// Unlike the non-default method, smaller numeric types will be coerced as needed.  
        /// </remarks>
        public static int GetInt32OrDefault ( this IDataRecord source, string name, int defaultValue )
        {
            int ordinal = VerifyGetOrdinal(source, name);

            return GetInt32OrDefault(source, ordinal, defaultValue);
        }

        /// <summary>Gets the column value.</summary>
        /// <param name="source">The source</param>
        /// <param name="name">The name of the column to retrieve.</param>
        /// <returns>The typed value.</returns>
        /// <exception cref="InvalidCastException">The column is not of the appropriate type.</exception>
        /// <exception cref="InvalidOperationException">The reader is closed.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="name"/> is not a valid field name.</exception>
        /// <remarks>
        /// To be consistent with the framework the method will throw an <see cref="InvalidCastException"/> if the column type does not
        /// exactly match the method type.
        /// </remarks>
        public static long GetInt64 ( this IDataRecord source, string name )
        {
            int ordinal = VerifyGetOrdinal(source, name);

            return source.GetInt64(ordinal);
        }

        /// <summary>Gets the column value or the default value if the column is not set.</summary>
        /// <param name="source">The source</param>
        /// <param name="ordinal">The zero-based ordinal.</param>
        /// <returns>The typed value or the default type value if the column was not set.</returns>
        /// <exception cref="InvalidCastException">The column is not of the appropriate type.</exception>
        /// <exception cref="InvalidOperationException">The reader is closed.</exception>
        /// <exception cref="IndexOutOfRangeException">
        /// <paramref name="ordinal"/> is less than zero.
        /// <para>-or-</para>
        /// <paramref name="ordinal"/> is too big for the reader.
        /// </exception>
        /// <remarks>
        /// Unlike the non-default method, smaller numeric types will be coerced as needed.  
        /// </remarks>
        public static long GetInt64OrDefault ( this IDataRecord source, int ordinal )
        {
            return GetInt64OrDefault(source, ordinal, 0);
        }

        /// <summary>Gets the column value or the default value if the column is not set.</summary>
        /// <param name="source">The source</param>
        /// <param name="ordinal">The zero-based ordinal.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The typed value or the default type value if the column was not set.</returns>
        /// <exception cref="InvalidCastException">The column is not of the appropriate type.</exception>
        /// <exception cref="InvalidOperationException">The reader is closed.</exception>
        /// <exception cref="IndexOutOfRangeException">
        /// <paramref name="ordinal"/> is less than zero.
        /// <para>-or-</para>
        /// <paramref name="ordinal"/> is too big for the reader.
        /// </exception>
        /// <remarks>
        /// Unlike the non-default method, smaller numeric types will be coerced as needed.  
        /// </remarks>
        public static long GetInt64OrDefault ( this IDataRecord source, int ordinal, long defaultValue )
        {
            if (!source.IsDBNull(ordinal))
                return Coerce<long>(source, ordinal);

            return defaultValue;
        }

        /// <summary>Gets the column value or the default value if the column is not set.</summary>
        /// <param name="source">The source</param>
        /// <param name="name">The name of the column to retrieve.</param>
        /// <returns>The typed value or the default type value if the column was not set.</returns>
        /// <exception cref="InvalidCastException">The column is not of the appropriate type.</exception>
        /// <exception cref="InvalidOperationException">The reader is closed.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="name"/> is not a valid field name.</exception>
        /// <remarks>
        /// Unlike the non-default method, smaller numeric types will be coerced as needed.  
        /// </remarks>
        public static long GetInt64OrDefault ( this IDataRecord source, string name )
        {
            return GetInt64OrDefault(source, name, 0);
        }

        /// <summary>Gets the column value or the default value if the column is not set.</summary>
        /// <param name="source">The source</param>
        /// <param name="name">The name of the column to retrieve.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The typed value or the default type value if the column was not set.</returns>
        /// <exception cref="InvalidCastException">The column is not of the appropriate type.</exception>
        /// <exception cref="InvalidOperationException">The reader is closed.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="name"/> is not a valid field name.</exception>
        /// <remarks>
        /// Unlike the non-default method, smaller numeric types will be coerced as needed.  
        /// </remarks>
        public static long GetInt64OrDefault ( this IDataRecord source, string name, long defaultValue )
        {
            int ordinal = VerifyGetOrdinal(source, name);

            return GetInt64OrDefault(source, ordinal, defaultValue);
        }

        /// <summary>Gets the column names.</summary>
        /// <param name="source">The source</param>
        /// <returns>An array of column names.</returns>
        /// <exception cref="InvalidOperationException">The reader is closed.</exception>
        public static string[] GetNames ( this IDataReader source )
        {
            if (source.IsClosed)
                throw new InvalidOperationException("The reader is closed.");

            int count = source.FieldCount;
            string[] names = new string[count];
            for (int nIdx = 0; nIdx < count; ++nIdx)
                names[nIdx] = source.GetName(nIdx);

            return names;
        }

        /// <summary>Gets the column value.</summary>
        /// <param name="source">The source</param>
        /// <param name="ordinal">The zero-based ordinal.</param>        
        /// <returns>The typed value.</returns>
        /// <exception cref="InvalidCastException">The column is not of the appropriate type.</exception>
        /// <exception cref="InvalidOperationException">The reader is closed.</exception>
        /// <exception cref="IndexOutOfRangeException">
        /// <paramref name="ordinal"/> is less than zero.
        /// <para>-or-</para>
        /// <paramref name="ordinal"/> is too big for the reader.        
        /// </exception>
        /// <remarks>
        /// To be consistent with the framework the method will throw an <see cref="InvalidCastException"/> if the column type does not
        /// exactly match the method type.
        /// </remarks>
        [CLSCompliant(false)]
        public static sbyte GetSByte ( this IDataRecord source, int ordinal )
        {
            return Convert.ToSByte(source.GetByte(ordinal));
        }

        /// <summary>Gets the column value.</summary>
        /// <param name="source">The source</param>
        /// <param name="name">The name of the column to retrieve.</param>
        /// <returns>The typed value.</returns>
        /// <exception cref="InvalidCastException">The column is not of the appropriate type.</exception>
        /// <exception cref="InvalidOperationException">The reader is closed.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="name"/> is not a valid field name.</exception>
        [CLSCompliant(false)]
        public static sbyte GetSByte ( this IDataRecord source, string name )
        {
            int ordinal = VerifyGetOrdinal(source, name);

            return source.GetSByte(ordinal);
        }

        /// <summary>Gets the column value or the default value if the column is not set.</summary>
        /// <param name="source">The source</param>
        /// <param name="ordinal">The zero-based ordinal.</param>
        /// <returns>The typed value or the default type value if the column was not set.</returns>
        /// <exception cref="InvalidCastException">The column is not of the appropriate type.</exception>
        /// <exception cref="InvalidOperationException">The reader is closed.</exception>
        /// <exception cref="IndexOutOfRangeException">
        /// <paramref name="ordinal"/> is less than zero.
        /// <para>-or-</para>
        /// <paramref name="ordinal"/> is too big for the reader.
        /// </exception>        
        [CLSCompliant(false)]
        public static sbyte GetSByteOrDefault ( this IDataRecord source, int ordinal )
        {
            return GetSByteOrDefault(source, ordinal, 0);
        }

        /// <summary>Gets the column value or the default value if the column is not set.</summary>
        /// <param name="source">The source</param>
        /// <param name="ordinal">The zero-based ordinal.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The typed value or the default type value if the column was not set.</returns>
        /// <exception cref="InvalidCastException">The column is not of the appropriate type.</exception>
        /// <exception cref="InvalidOperationException">The reader is closed.</exception>
        /// <exception cref="IndexOutOfRangeException">
        /// <paramref name="ordinal"/> is less than zero.
        /// <para>-or-</para>
        /// <paramref name="ordinal"/> is too big for the reader.
        /// </exception>        
        [CLSCompliant(false)]
        public static sbyte GetSByteOrDefault ( this IDataRecord source, int ordinal, sbyte defaultValue )
        {
            if (!source.IsDBNull(ordinal))
                return source.GetSByte(ordinal);

            return defaultValue;
        }

        /// <summary>Gets the column value or the default value if the column is not set.</summary>
        /// <param name="source">The source</param>
        /// <param name="name">The name of the column to retrieve.</param>
        /// <returns>The typed value or the default type value if the column was not set.</returns>
        /// <exception cref="InvalidCastException">The column is not of the appropriate type.</exception>
        /// <exception cref="InvalidOperationException">The reader is closed.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="name"/> is not a valid field name.</exception>        
        [CLSCompliant(false)]
        public static sbyte GetSByteOrDefault ( this IDataRecord source, string name )
        {
            return GetSByteOrDefault(source, name, 0);
        }

        /// <summary>Gets the column value or the default value if the column is not set.</summary>
        /// <param name="source">The source</param>
        /// <param name="name">The name of the column to retrieve.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The typed value or the default type value if the column was not set.</returns>
        /// <exception cref="InvalidCastException">The column is not of the appropriate type.</exception>
        /// <exception cref="InvalidOperationException">The reader is closed.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="name"/> is not a valid field name.</exception>        
        [CLSCompliant(false)]
        public static sbyte GetSByteOrDefault ( this IDataRecord source, string name, sbyte defaultValue )
        {
            int ordinal = VerifyGetOrdinal(source, name);

            return GetSByteOrDefault(source, ordinal, defaultValue);
        }

        /// <summary>Gets the column value or the default value if the column is not set.</summary>
        /// <param name="source">The source</param>
        /// <param name="ordinal">The zero-based ordinal.</param>
        /// <returns>The typed value or the default type value if the column was not set.</returns>
        /// <exception cref="InvalidCastException">The column is not of the appropriate type.</exception>
        /// <exception cref="InvalidOperationException">The reader is closed.</exception>
        /// <exception cref="IndexOutOfRangeException">
        /// <paramref name="ordinal"/> is less than zero.
        /// <para>-or-</para>
        /// <paramref name="ordinal"/> is too big for the reader.
        /// </exception>
        /// <remarks>
        /// Unlike the non-default method, smaller numeric types will be coerced as needed.  
        /// </remarks>
        public static float GetSingleOrDefault ( this IDataRecord source, int ordinal )
        {
            return GetSingleOrDefault(source, ordinal, 0);
        }

        /// <summary>Gets the column value or the default value if the column is not set.</summary>
        /// <param name="source">The source</param>
        /// <param name="ordinal">The zero-based ordinal.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The typed value or the default type value if the column was not set.</returns>
        /// <exception cref="InvalidCastException">The column is not of the appropriate type.</exception>
        /// <exception cref="InvalidOperationException">The reader is closed.</exception>
        /// <exception cref="IndexOutOfRangeException">
        /// <paramref name="ordinal"/> is less than zero.
        /// <para>-or-</para>
        /// <paramref name="ordinal"/> is too big for the reader.
        /// </exception>
        /// <remarks>
        /// Unlike the non-default method, smaller numeric types will be coerced as needed.  
        /// </remarks>
        public static float GetSingleOrDefault ( this IDataRecord source, int ordinal, float defaultValue )
        {
            if (!source.IsDBNull(ordinal))
                return Coerce<float>(source, ordinal);

            return defaultValue;
        }

        /// <summary>Gets the column value or the default value if the column is not set.</summary>
        /// <param name="source">The source</param>
        /// <param name="name">The name of the column to retrieve.</param>
        /// <returns>The typed value or the default type value if the column was not set.</returns>
        /// <exception cref="InvalidCastException">The column is not of the appropriate type.</exception>
        /// <exception cref="InvalidOperationException">The reader is closed.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="name"/> is not a valid field name.</exception>
        /// <remarks>
        /// Unlike the non-default method, smaller numeric types will be coerced as needed.  
        /// </remarks>
        public static float GetSingleOrDefault ( this IDataRecord source, string name )
        {
            return GetSingleOrDefault(source, name, 0);
        }

        /// <summary>Gets the column value or the default value if the column is not set.</summary>
        /// <param name="source">The source</param>
        /// <param name="name">The name of the column to retrieve.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The typed value or the default type value if the column was not set.</returns>
        /// <exception cref="InvalidCastException">The column is not of the appropriate type.</exception>
        /// <exception cref="InvalidOperationException">The reader is closed.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="name"/> is not a valid field name.</exception>
        /// <remarks>
        /// Unlike the non-default method, smaller numeric types will be coerced as needed.  
        /// </remarks>
        public static float GetSingleOrDefault ( this IDataRecord source, string name, float defaultValue )
        {
            int ordinal = VerifyGetOrdinal(source, name);

            return GetSingleOrDefault(source, ordinal, defaultValue);
        }

        /// <summary>Gets the column value.</summary>
        /// <param name="source">The source</param>
        /// <param name="name">The name of the column to retrieve.</param>
        /// <returns>The typed value.</returns>
        /// <exception cref="InvalidCastException">The column is not of the appropriate type.</exception>
        /// <exception cref="InvalidOperationException">The reader is closed.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="name"/> is not a valid field name.</exception>
        /// <remarks>
        /// To be consistent with the framework the method will throw an <see cref="InvalidCastException"/> if the column type does not
        /// exactly match the method type.
        /// </remarks>
        public static string GetString ( this IDataRecord source, string name )
        {
            int ordinal = VerifyGetOrdinal(source, name);

            return source.GetString(ordinal);
        }

        /// <summary>Gets the column value or the default value if the column is not set.</summary>
        /// <param name="source">The source</param>
        /// <param name="ordinal">The zero-based ordinal.</param>
        /// <returns>The typed value or an empty string if the column was not set.</returns>
        /// <exception cref="InvalidCastException">The column is not of the appropriate type.</exception>
        /// <exception cref="InvalidOperationException">The reader is closed.</exception>
        /// <exception cref="IndexOutOfRangeException">
        /// <paramref name="ordinal"/> is less than zero.
        /// <para>-or-</para>
        /// <paramref name="ordinal"/> is too big for the reader.
        /// </exception>
        /// <remarks>
        /// Unlike the non-default method, all column types can be returned as a string.
        /// </remarks>
        public static string GetStringOrDefault ( this IDataRecord source, int ordinal )
        {
            return GetStringOrDefault(source, ordinal, "");
        }

        /// <summary>Gets the column value or the default value if the column is not set.</summary>
        /// <param name="source">The source</param>
        /// <param name="ordinal">The zero-based ordinal.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The typed value or an empty string if the column was not set.</returns>
        /// <exception cref="InvalidCastException">The column is not of the appropriate type.</exception>
        /// <exception cref="InvalidOperationException">The reader is closed.</exception>
        /// <exception cref="IndexOutOfRangeException">
        /// <paramref name="ordinal"/> is less than zero.
        /// <para>-or-</para>
        /// <paramref name="ordinal"/> is too big for the reader.
        /// </exception>
        /// <remarks>
        /// Unlike the non-default method, all column types can be returned as a string.
        /// </remarks>
        public static string GetStringOrDefault ( this IDataRecord source, int ordinal, string defaultValue )
        {
            if (!source.IsDBNull(ordinal))
            {
                var value = source.GetValue(ordinal);

                return value.ToString();
            };

            return defaultValue;
        }

        /// <summary>Gets the column value or the default value if the column is not set.</summary>
        /// <param name="source">The source</param>
        /// <param name="name">The name of the column to retrieve.</param>
        /// <returns>The typed value or an empty string if the column was not set.</returns>
        /// <exception cref="InvalidCastException">The column is not of the appropriate type.</exception>
        /// <exception cref="InvalidOperationException">The reader is closed.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="name"/> is not a valid field name.</exception>
        public static string GetStringOrDefault ( this IDataRecord source, string name )
        {
            return GetStringOrDefault(source, name, "");
        }

        /// <summary>Gets the column value or the default value if the column is not set.</summary>
        /// <param name="source">The source</param>
        /// <param name="name">The name of the column to retrieve.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The typed value or an empty string if the column was not set.</returns>
        /// <exception cref="InvalidCastException">The column is not of the appropriate type.</exception>
        /// <exception cref="InvalidOperationException">The reader is closed.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="name"/> is not a valid field name.</exception>
        public static string GetStringOrDefault ( this IDataRecord source, string name, string defaultValue )
        {
            int ordinal = VerifyGetOrdinal(source, name);

            return GetStringOrDefault(source, ordinal, defaultValue);
        }

        /// <summary>Gets the column value.</summary>
        /// <param name="source">The source</param>
        /// <param name="ordinal">The zero-based ordinal.</param>        
        /// <returns>The typed value.</returns>
        /// <exception cref="InvalidCastException">The column is not of the appropriate type.</exception>
        /// <exception cref="InvalidOperationException">The reader is closed.</exception>
        /// <exception cref="IndexOutOfRangeException">
        /// <paramref name="ordinal"/> is less than zero.
        /// <para>-or-</para>
        /// <paramref name="ordinal"/> is too big for the reader.
        /// </exception>
        /// <remarks>
        /// To be consistent with the framework the method will throw an <see cref="InvalidCastException"/> if the column type does not
        /// exactly match the method type.
        /// </remarks>
        [CLSCompliant(false)]
        public static ushort GetUInt16 ( this IDataRecord source, int ordinal )
        {
            return Convert.ToUInt16(source.GetInt16(ordinal));
        }

        /// <summary>Gets the column value.</summary>
        /// <param name="source">The source</param>
        /// <param name="name">The name of the column to retrieve.</param>
        /// <returns>The typed value.</returns>
        /// <exception cref="InvalidCastException">The column is not of the appropriate type.</exception>
        /// <exception cref="InvalidOperationException">The reader is closed.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="name"/> is not a valid field name.</exception>
        /// <remarks>
        /// To be consistent with the framework the method will throw an <see cref="InvalidCastException"/> if the column type does not
        /// exactly match the method type.
        /// </remarks>
        [CLSCompliant(false)]
        public static ushort GetUInt16 ( this IDataRecord source, string name )
        {
            int ordinal = VerifyGetOrdinal(source, name);

            return source.GetUInt16(ordinal);
        }

        /// <summary>Gets the column value or the default value if the column is not set.</summary>
        /// <param name="source">The source</param>
        /// <param name="ordinal">The zero-based ordinal.</param>
        /// <returns>The typed value or the default type value if the column was not set.</returns>
        /// <exception cref="InvalidCastException">The column is not of the appropriate type.</exception>
        /// <exception cref="InvalidOperationException">The reader is closed.</exception>
        /// <exception cref="IndexOutOfRangeException">
        /// <paramref name="ordinal"/> is less than zero.
        /// <para>-or-</para>
        /// <paramref name="ordinal"/> is too big for the reader.
        /// </exception>
        /// <remarks>
        /// Unlike the non-default method, smaller numeric types will be coerced as needed.  
        /// </remarks>
        [CLSCompliant(false)]
        public static ushort GetUInt16OrDefault ( this IDataRecord source, int ordinal )
        {
            return GetUInt16OrDefault(source, ordinal, 0);
        }

        /// <summary>Gets the column value or the default value if the column is not set.</summary>
        /// <param name="source">The source</param>
        /// <param name="ordinal">The zero-based ordinal.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The typed value or the default type value if the column was not set.</returns>
        /// <exception cref="InvalidCastException">The column is not of the appropriate type.</exception>
        /// <exception cref="InvalidOperationException">The reader is closed.</exception>
        /// <exception cref="IndexOutOfRangeException">
        /// <paramref name="ordinal"/> is less than zero.
        /// <para>-or-</para>
        /// <paramref name="ordinal"/> is too big for the reader.
        /// </exception>
        /// <remarks>
        /// Unlike the non-default method, smaller numeric types will be coerced as needed.  
        /// </remarks>
        [CLSCompliant(false)]
        public static ushort GetUInt16OrDefault ( this IDataRecord source, int ordinal, ushort defaultValue )
        {
            if (!source.IsDBNull(ordinal))
                return Coerce<ushort>(source, ordinal);

            return defaultValue;
        }

        /// <summary>Gets the column value or the default value if the column is not set.</summary>
        /// <param name="source">The source</param>
        /// <param name="name">The name of the column to retrieve.</param>
        /// <returns>The typed value or the default type value if the column was not set.</returns>
        /// <exception cref="InvalidCastException">The column is not of the appropriate type.</exception>
        /// <exception cref="InvalidOperationException">The reader is closed.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="name"/> is not a valid field name.</exception>
        /// <remarks>
        /// Unlike the non-default method, smaller numeric types will be coerced as needed.  
        /// </remarks>
        [CLSCompliant(false)]
        public static ushort GetUInt16OrDefault ( this IDataRecord source, string name )
        {
            return GetUInt16OrDefault(source, name, 0);
        }

        /// <summary>Gets the column value or the default value if the column is not set.</summary>
        /// <param name="source">The source</param>
        /// <param name="name">The name of the column to retrieve.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The typed value or the default type value if the column was not set.</returns>
        /// <exception cref="InvalidCastException">The column is not of the appropriate type.</exception>
        /// <exception cref="InvalidOperationException">The reader is closed.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="name"/> is not a valid field name.</exception>
        /// <remarks>
        /// Unlike the non-default method, smaller numeric types will be coerced as needed.  
        /// </remarks>
        [CLSCompliant(false)]
        public static ushort GetUInt16OrDefault ( this IDataRecord source, string name, ushort defaultValue )
        {
            int ordinal = VerifyGetOrdinal(source, name);

            return GetUInt16OrDefault(source, ordinal, defaultValue);
        }

        /// <summary>Gets the column value.</summary>
        /// <param name="source">The source</param>
        /// <param name="ordinal">The zero-based ordinal.</param>        
        /// <returns>The typed value.</returns>
        /// <exception cref="InvalidCastException">The column is not of the appropriate type.</exception>
        /// <exception cref="InvalidOperationException">The reader is closed.</exception>
        /// <exception cref="IndexOutOfRangeException">
        /// <paramref name="ordinal"/> is less than zero.
        /// <para>-or-</para>
        /// <paramref name="ordinal"/> is too big for the reader.
        /// </exception>
        /// <remarks>
        /// To be consistent with the framework the method will throw an <see cref="InvalidCastException"/> if the column type does not
        /// exactly match the method type.
        /// </remarks>
        [CLSCompliant(false)]
        public static uint GetUInt32 ( this IDataRecord source, int ordinal )
        {
            return Convert.ToUInt32(source.GetInt32(ordinal));
        }

        /// <summary>Gets the column value.</summary>
        /// <param name="source">The source</param>
        /// <param name="name">The name of the column to retrieve.</param>
        /// <returns>The typed value.</returns>
        /// <exception cref="InvalidCastException">The column is not of the appropriate type.</exception>
        /// <exception cref="InvalidOperationException">The reader is closed.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="name"/> is not a valid field name.</exception>
        /// <remarks>
        /// To be consistent with the framework the method will throw an <see cref="InvalidCastException"/> if the column type does not
        /// exactly match the method type.
        /// </remarks>
        [CLSCompliant(false)]
        public static uint GetUInt32 ( this IDataRecord source, string name )
        {
            int ordinal = VerifyGetOrdinal(source, name);

            return source.GetUInt32(ordinal);
        }

        /// <summary>Gets the column value or the default value if the column is not set.</summary>
        /// <param name="source">The source</param>
        /// <param name="ordinal">The zero-based ordinal.</param>
        /// <returns>The typed value or the default type value if the column was not set.</returns>
        /// <exception cref="InvalidCastException">The column is not of the appropriate type.</exception>
        /// <exception cref="InvalidOperationException">The reader is closed.</exception>
        /// <exception cref="IndexOutOfRangeException">
        /// <paramref name="ordinal"/> is less than zero.
        /// <para>-or-</para>
        /// <paramref name="ordinal"/> is too big for the reader.
        /// </exception>
        /// <remarks>
        /// Unlike the non-default method, smaller numeric types will be coerced as needed.  
        /// </remarks>
        [CLSCompliant(false)]
        public static uint GetUInt32OrDefault ( this IDataRecord source, int ordinal )
        {
            return GetUInt32OrDefault(source, ordinal, 0);
        }

        /// <summary>Gets the column value or the default value if the column is not set.</summary>
        /// <param name="source">The source</param>
        /// <param name="ordinal">The zero-based ordinal.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The typed value or the default type value if the column was not set.</returns>
        /// <exception cref="InvalidCastException">The column is not of the appropriate type.</exception>
        /// <exception cref="InvalidOperationException">The reader is closed.</exception>
        /// <exception cref="IndexOutOfRangeException">
        /// <paramref name="ordinal"/> is less than zero.
        /// <para>-or-</para>
        /// <paramref name="ordinal"/> is too big for the reader.
        /// </exception>
        /// <remarks>
        /// Unlike the non-default method, smaller numeric types will be coerced as needed.  
        /// </remarks>
        [CLSCompliant(false)]
        public static uint GetUInt32OrDefault ( this IDataRecord source, int ordinal, uint defaultValue )
        {
            if (!source.IsDBNull(ordinal))
                return Coerce<uint>(source, ordinal);

            return defaultValue;
        }

        /// <summary>Gets the column value or the default value if the column is not set.</summary>
        /// <param name="source">The source</param>
        /// <param name="name">The name of the column to retrieve.</param>
        /// <returns>The typed value or the default type value if the column was not set.</returns>
        /// <exception cref="InvalidCastException">The column is not of the appropriate type.</exception>
        /// <exception cref="InvalidOperationException">The reader is closed.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="name"/> is not a valid field name.</exception>
        /// <remarks>
        /// Unlike the non-default method, smaller numeric types will be coerced as needed.  
        /// </remarks>
        [CLSCompliant(false)]
        public static uint GetUInt32OrDefault ( this IDataRecord source, string name )
        {
            return GetUInt32OrDefault(source, name, 0);
        }

        /// <summary>Gets the column value or the default value if the column is not set.</summary>
        /// <param name="source">The source</param>
        /// <param name="name">The name of the column to retrieve.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The typed value or the default type value if the column was not set.</returns>
        /// <exception cref="InvalidCastException">The column is not of the appropriate type.</exception>
        /// <exception cref="InvalidOperationException">The reader is closed.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="name"/> is not a valid field name.</exception>
        /// <remarks>
        /// Unlike the non-default method, smaller numeric types will be coerced as needed.  
        /// </remarks>
        [CLSCompliant(false)]
        public static uint GetUInt32OrDefault ( this IDataRecord source, string name, uint defaultValue )
        {
            int ordinal = VerifyGetOrdinal(source, name);

            return GetUInt32OrDefault(source, ordinal, defaultValue);
        }

        /// <summary>Gets the column value.</summary>
        /// <param name="source">The source</param>
        /// <param name="ordinal">The zero-based ordinal.</param>        
        /// <returns>The typed value.</returns>
        /// <exception cref="InvalidCastException">The column is not of the appropriate type.</exception>
        /// <exception cref="InvalidOperationException">The reader is closed.</exception>
        /// <exception cref="IndexOutOfRangeException">
        /// <paramref name="ordinal"/> is less than zero.
        /// <para>-or-</para>
        /// <paramref name="ordinal"/> is too big for the reader.
        /// </exception>
        /// <remarks>
        /// To be consistent with the framework the method will throw an <see cref="InvalidCastException"/> if the column type does not
        /// exactly match the method type.
        /// </remarks>
        [CLSCompliant(false)]
        public static ulong GetUInt64 ( this IDataRecord source, int ordinal )
        {
            return Convert.ToUInt64(source.GetInt64(ordinal));
        }

        /// <summary>Gets the column value.</summary>
        /// <param name="source">The source</param>
        /// <param name="name">The name of the column to retrieve.</param>
        /// <returns>The typed value.</returns>
        /// <exception cref="InvalidCastException">The column is not of the appropriate type.</exception>
        /// <exception cref="InvalidOperationException">The reader is closed.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="name"/> is not a valid field name.</exception>
        /// <remarks>
        /// To be consistent with the framework the method will throw an <see cref="InvalidCastException"/> if the column type does not
        /// exactly match the method type.
        /// </remarks>
        [CLSCompliant(false)]
        public static ulong GetUInt64 ( this IDataRecord source, string name )
        {
            int ordinal = VerifyGetOrdinal(source, name);

            return source.GetUInt64(ordinal);
        }

        /// <summary>Gets the column value or the default value if the column is not set.</summary>
        /// <param name="source">The source</param>
        /// <param name="ordinal">The zero-based ordinal.</param>
        /// <returns>The typed value or the default type value if the column was not set.</returns>
        /// <exception cref="InvalidCastException">The column is not of the appropriate type.</exception>
        /// <exception cref="InvalidOperationException">The reader is closed.</exception>
        /// <exception cref="IndexOutOfRangeException">
        /// <paramref name="ordinal"/> is less than zero.
        /// <para>-or-</para>
        /// <paramref name="ordinal"/> is too big for the reader.
        /// </exception>
        /// <remarks>
        /// Unlike the non-default method, smaller numeric types will be coerced as needed.  
        /// </remarks>
        [CLSCompliant(false)]
        public static ulong GetUInt64OrDefault ( this IDataRecord source, int ordinal )
        {
            return GetUInt64OrDefault(source, ordinal, 0);
        }

        /// <summary>Gets the column value or the default value if the column is not set.</summary>
        /// <param name="source">The source</param>
        /// <param name="ordinal">The zero-based ordinal.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The typed value or the default type value if the column was not set.</returns>
        /// <exception cref="InvalidCastException">The column is not of the appropriate type.</exception>
        /// <exception cref="InvalidOperationException">The reader is closed.</exception>
        /// <exception cref="IndexOutOfRangeException">
        /// <paramref name="ordinal"/> is less than zero.
        /// <para>-or-</para>
        /// <paramref name="ordinal"/> is too big for the reader.
        /// </exception>
        /// <remarks>
        /// Unlike the non-default method, smaller numeric types will be coerced as needed.  
        /// </remarks>
        /// <example>
        /// Refer to <see cref="DataReaderExtensions"/> for an example of how to use this method.
        /// </example>
        [CLSCompliant(false)]
        public static ulong GetUInt64OrDefault ( this IDataRecord source, int ordinal, ulong defaultValue )
        {
            if (!source.IsDBNull(ordinal))
                return Coerce<ulong>(source, ordinal);

            return defaultValue;
        }

        /// <summary>Gets the column value or the default value if the column is not set.</summary>
        /// <param name="source">The source</param>
        /// <param name="name">The name of the column to retrieve.</param>
        /// <returns>The typed value or the default type value if the column was not set.</returns>
        /// <exception cref="InvalidCastException">The column is not of the appropriate type.</exception>
        /// <exception cref="InvalidOperationException">The reader is closed.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="name"/> is not a valid field name.</exception>
        /// <remarks>
        /// Unlike the non-default method, smaller numeric types will be coerced as needed.  
        /// </remarks>
        [CLSCompliant(false)]
        public static ulong GetUInt64OrDefault ( this IDataRecord source, string name )
        {
            return GetUInt64OrDefault(source, name, 0);
        }

        /// <summary>Gets the column value or the default value if the column is not set.</summary>
        /// <param name="source">The source</param>
        /// <param name="name">The name of the column to retrieve.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The typed value or the default type value if the column was not set.</returns>
        /// <exception cref="InvalidCastException">The column is not of the appropriate type.</exception>
        /// <exception cref="InvalidOperationException">The reader is closed.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="name"/> is not a valid field name.</exception>
        /// <remarks>
        /// Unlike the non-default method, smaller numeric types will be coerced as needed.  
        /// </remarks>
        [CLSCompliant(false)]
        public static ulong GetUInt64OrDefault ( this IDataRecord source, string name, ulong defaultValue )
        {
            int ordinal = VerifyGetOrdinal(source, name);

            return GetUInt64OrDefault(source, ordinal, defaultValue);
        }
        
        /// <summary>Gets the value of the specified column.</summary>
        /// <param name="source">The source</param>
        /// <param name="name">The name of the column to retrieve.</param>
        /// <returns>The value of the column.  If the column is NULL then it returns <see langword="null"/>.</returns>
        /// <exception cref="InvalidOperationException">The reader is closed.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="name"/> is not a valid field name.</exception>
        public static object GetValue ( this IDataRecord source, string name )
        {
            int ordinal = VerifyGetOrdinal(source, name);

            return source.GetValue(ordinal);
        }

        /// <summary>Gets a flag indicating if the specified column is NULL.</summary>
        /// <param name="source">The source</param>
        /// <param name="name">The name of the column to retrieve.</param>
        /// <returns><see langword="true"/> if the column is NULL.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="name"/> is not a valid field name.</exception>
        public static bool IsDBNull ( this IDataRecord source, string name )
        {
            int ordinal = VerifyGetOrdinal(source, name);

            return source.IsDBNull(ordinal);
        }

        #region Private Members

        private static T Coerce<T>( IDataRecord source, int ordinal ) where T : struct
        {
            var value = source.GetValue(ordinal);

            return TypeConversion.Coerce<T>(value);
        }

        private static int VerifyGetOrdinal ( IDataRecord source, string name )
        {
            int ordinal = source.GetOrdinal(name);
            if (ordinal < 0)
                throw new ArgumentException("Field does not exist.", "name");

            return ordinal;
        }
        #endregion

    }

}
