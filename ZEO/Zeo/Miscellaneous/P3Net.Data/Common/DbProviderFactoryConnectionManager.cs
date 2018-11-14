using System;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace P3Net.Data.Common
{
    /// <summary>Provides an implementation of <see cref="ConnectionManager"/> using a <see cref="DbProviderFactory"/>.</summary>
    /// <remarks>
    /// Derived classes do not need to implement any of the base methods but can do so to optimize performance.
    /// </remarks>	
    /// <preliminary />
    public class DbProviderFactoryConnectionManager : ConnectionManager
    {
        #region Construction

        /// <summary>Initializes an instance of the <see cref="DbProviderFactoryConnectionManager"/> class.</summary>
        /// <param name="factory">The underlying factory to use.</param>
        /// <param name="connectionString">The connection string to use.</param>
        /// <exception cref="ArgumentNullException"><paramref name="factory"/> is <see langword="null"/>.</exception>		
        protected DbProviderFactoryConnectionManager ( DbProviderFactory factory, string connectionString ) : base(connectionString)
        {
            if (factory == null)
                throw new ArgumentNullException("factory");

            Factory = factory;

            m_schema = new Lazy<SchemaInformation>(CallLoadSchema);
        }
        #endregion

        /// <summary>Gets the underlying database factory.</summary>
        protected DbProviderFactory Factory { get; private set; }

        /// <summary>Creates a connection given a connection string.</summary>
        /// <param name="connectionString">The connection string to use.</param>
        /// <returns>The underlying connection object.</returns>
        protected override DbConnection CreateConnectionBase ( string connectionString )
        {
            DbConnection conn = null;
            try
            {
                conn = Factory.CreateConnection();
                conn.ConnectionString = connectionString;

                return conn;
            } catch (Exception)
            {
                if (conn != null)
                    conn.Dispose();

                throw;
            };
        }

        /// <summary>Creates a data adapter.</summary>
        /// <returns>The underlying data adapter.</returns>
        protected override DbDataAdapter CreateDataAdapterBase ()
        {
            return Factory.CreateDataAdapter();
        }
        
        /// <summary>Creates a command.</summary>
        /// <param name="command">The base command to use.</param>
        /// <returns>The underlying command.</returns>
        protected override DbCommand CreateCommandBase ( DataCommand command )
        {
            DbCommand cmdDb = Factory.CreateCommand();

            cmdDb.CommandText = command.CommandText;
            cmdDb.CommandTimeout = (int)command.CommandTimeout.TotalSeconds;
            cmdDb.CommandType = command.CommandType;
            cmdDb.UpdatedRowSource = command.UpdatedRowSource;

            //Parameters
            foreach (var parm in command.Parameters)
            {
                cmdDb.Parameters.Add(CreateParameterBase(parm, cmdDb));
            };

            return cmdDb;
        }

        /// <summary>Creates the parameter for a command.</summary>
        /// <param name="source">The source</param>
        /// <param name="command">The underlying command being created.</param>
        /// <returns>The underlying parameter.</returns>
        /// <remarks>
        /// This method uses the database schema to ensure that the parameter is properly formatted.
        /// </remarks>
        protected virtual DbParameter CreateParameterBase (DataParameter source, DbCommand command)
        {
            DbParameter target = command.CreateParameter();

            target.ParameterName = FormatParameterName(source.Name);
            target.DbType = source.DbType;
            target.Direction = source.Direction;
            target.Size = source.Size;
            target.SourceColumn = source.SourceColumn;
            target.SourceVersion = source.SourceVersion;

            //Copy the value only if needed
            switch (source.Direction)
            {
                //Ensure that the value is NULL if it is not set otherwise sproc calls might fail
                case ParameterDirection.InputOutput:
                case ParameterDirection.Input: target.Value = source.Value ?? DBNull.Value; break;
            };

            return target;
        }

        /// <summary>Creates a transaction.</summary>
        /// <param name="connection">The connection used for the transaction.</param>
        /// <param name="level">The isolation level to use.</param>
        /// <returns>The underlying transaction.</returns>
        protected override DbTransaction CreateTransactionBase (DbConnection connection, IsolationLevel level)
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();

            return connection.BeginTransaction(level);
        }

        /// <summary>Formats a parameter name in the format supported by the provider.</summary>
        /// <param name="originalName">The parameter name.</param>
        /// <returns>The formatted string.</returns>
        /// <remarks>
        /// The default implementation retrieves the provider schema and formats the parameter based on the schema.
        /// </remarks>
        protected virtual string FormatParameterName (string originalName)
        {
            var schema = GetSchema();

            //Apply the formatting based on the schema
            if (schema.HasParameterPrefix && !originalName.StartsWith(schema.ParameterFormatPrefix))
                originalName = schema.ParameterFormatPrefix + originalName;
            if (schema.HasParameterSuffix && !originalName.EndsWith(schema.ParameterFormatSuffix))
                originalName = originalName + schema.ParameterFormatSuffix;

            return originalName;
        }

        /// <summary>Gets the schema defined by the underlying provider.</summary>
        /// <returns>The schema information.</returns>
        protected SchemaInformation GetSchema ()
        {
            return m_schema.Value;
        }

        /// <summary>Loads schema information about the provider.</summary>
        /// <returns>The schema information.</returns>
        /// <remarks>
        /// The default implementation retrieves the schema information from the database.
        /// </remarks>
        protected virtual SchemaInformation LoadSchema ()
        {
            var schema = new SchemaInformation();

            //Open the DB
            using (var conn = CreateConnectionBase(ConnectionString))
            {
                conn.Open();

                //Get the Schema
                var dt = conn.GetSchema("DataSourceInformation");
                if ((dt != null) && (dt.Rows.Count > 0))
                {
                    DataRow dr = dt.Rows[0];

                    //Get the parameter formatParameterName
                    if (dt.Columns.Contains("ParameterMarkerFormat"))
                        schema.ParameterFormat = dr.GetStringValueOrDefault("ParameterMarkerFormat");
                };
            };

            return schema;
        }

        #region Private Members

        //Used to call the virtual method to load the schema
        private SchemaInformation CallLoadSchema ()
        {
            return LoadSchema();
        }

        //Used to filter and format parameter names (the prefix and suffix contain the 
        //schema-defined parameter layout for parameter names.  
        //TODO: Optimize this such that it only fetch it once per connection manager type
        private readonly Lazy<SchemaInformation> m_schema;
        #endregion
    }
}
