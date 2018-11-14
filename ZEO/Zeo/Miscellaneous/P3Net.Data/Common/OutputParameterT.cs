using System;
using System.Data;

namespace P3Net.Data.Common
{
    /// <summary>Represents an output parameter.</summary>
    /// <typeparam name="T">The CLR type of the parameter.</typeparam>
    /// <seealso cref="DbTypeMapper"/>
    public class OutputParameter<T> : DataParameter<T>
    {
        #region Construction

        /// <summary>Initializes an instance of the <see cref="OutputParameter{T}"/> class.</summary>
        /// <param name="name">The name of the parameter.</param>
        public OutputParameter(string name) : base(name, ParameterDirection.Output)
        { }
        #endregion

        /// <summary>Gets the current typed value.</summary>
        /// <returns>The typed parameter value.</returns>
        public T GetValue()
        {
            return TypedValue;
        }
    }
}
