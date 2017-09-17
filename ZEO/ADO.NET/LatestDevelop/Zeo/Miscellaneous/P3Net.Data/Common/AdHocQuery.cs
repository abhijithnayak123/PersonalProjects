using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;

namespace P3Net.Data.Common
{
    /// <summary>Represents an adhoc query command.</summary>
    public class AdhocQuery : DataCommand
    {
        #region Construction

        /// <summary>Initializes an instance of the <see cref="AdhocQuery"/> class.</summary>
        /// <param name="commandText">The query text.</param>
        /// <exception cref="ArgumentNullException"><paramref name="commandText"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="commandText"/> is empty.</exception>
        public AdhocQuery (string commandText) : base(commandText, CommandType.Text)
        {
        }
        #endregion
    }
}
