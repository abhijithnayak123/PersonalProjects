using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;

namespace P3Net.Data.Common
{
    /// <summary>Represents a stored procedure command.</summary>
    public class StoredProcedure : DataCommand
    {
        #region Construction

        /// <summary>Iniitializes an instance of the <see cref="StoredProcedure"/> class.</summary>
        /// <param name="name">The name of the stored procedure.</param>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="name"/> is empty.</exception>
        public StoredProcedure (string name) : base(name, CommandType.StoredProcedure)
        {
        }
        #endregion

        /// <summary>Gets the return value after the stored procedure has been run.</summary>
        public int ReturnValue { get; internal set; }
    }
}
