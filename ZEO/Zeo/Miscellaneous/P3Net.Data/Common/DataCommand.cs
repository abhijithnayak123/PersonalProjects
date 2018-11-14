using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace P3Net.Data.Common
{
    /// <summary>Represents a command to be executed against a database.</summary>
    public class DataCommand 
    {
        #region Construction

        /// <summary>Initializes an instance of the <see cref="DataCommand"/> class.</summary>
        /// <param name="commandText">The command text.</param>
        /// <param name="type">The type of command.</param>
        /// <exception cref="ArgumentNullException"><paramref name="commandText"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="commandText"/> is empty.</exception>
        public DataCommand ( string commandText, CommandType type )
        {
            if (commandText == null)
                throw new ArgumentNullException("commandText");
            if (String.IsNullOrEmpty(commandText))
                throw new ArgumentException("Command cannot be empty.", "commandText");

            CommandText = commandText;
            CommandType = type;
            Parameters = new DataParameterCollection();
        }        
        #endregion

        /// <summary>Gets the command to execute.</summary>
        public string CommandText { get; private set; }

        /// <summary>Gets or sets the timeout for the command.</summary>
        /// <exception cref="ArgumentOutOfRangeException">The timeout is less than zero.</exception>
        /// <value>The default is zero.</value>
        public TimeSpan CommandTimeout
        {
            get { return m_commandTimeout; }
            set 
            {
                if (value < TimeSpan.Zero)
                    throw new ArgumentOutOfRangeException("value", "Timeout cannot be less than zero.");
                
                m_commandTimeout = value; 
            }
        }

        /// <summary>Gets the type of command to execute.</summary>
        public CommandType CommandType { get; private set; }

        /// <summary>Gets the parameters associated with the command.</summary>
        public DataParameterCollection Parameters { get; private set; }

        /// <summary>Gets or sets how results are applied to a <see cref="DataRow"/> for Update commands.</summary>
        public UpdateRowSource UpdatedRowSource { get; set; }
        
        /// <summary>Gets a string representation of the class.</summary>
        /// <returns>A string representing the class.</returns>
        public override string ToString()
        {
            return CommandText;
        }

        /// <summary>Adds parameters to the command.</summary>
        /// <param name="parameters">The parameters to add.</param>
        /// <returns>The updated command.</returns>
        public DataCommand WithParameters (params DataParameter[] parameters)
        {
            if (parameters != null)
            {
                foreach (var parameter in parameters)
                    Parameters.Add(parameter);
            };

            return this;
        }

        #region Private Members

        private TimeSpan m_commandTimeout;

        #endregion 
    }
}