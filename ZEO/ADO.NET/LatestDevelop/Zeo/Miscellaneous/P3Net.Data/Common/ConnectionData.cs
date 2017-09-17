using System;
using System.Data;
using System.Data.Common;

namespace P3Net.Data.Common
{
    /// <summary>Provides connection information for classes deriving from <see cref="ConnectionManager"/>.</summary>
    public sealed class ConnectionData : IDisposable
    {
        #region Construction

        internal ConnectionData ( DbConnection connection )
        {
            m_connection = connection;
        }

        internal ConnectionData ( DbTransaction transaction )
        {
            m_transaction = transaction;
        }
        #endregion

        /// <summary>Gets the underlying connection.</summary>
        public DbConnection Connection
        {
            get { return m_connection ?? m_transaction.Connection; }
        }

        /// <summary>Gets the underlying transaction.</summary>
        public DbTransaction Transaction
        {
            get { return m_transaction; }
        }
        
        /// <summary>Detaches the connection from the object so it won't be closed.</summary>
        /// <returns>The connection.</returns>
        public DbConnection Detach ()
        {
            var conn = m_connection;
            m_connection = null;

            return conn;
        }

        /// <summary>Disposes of the instance.</summary>
        public void Dispose ()
        {
            Dispose(true);
        }

        /// <summary>Opens the connection if it is not already opened.</summary>
        public void Open ()
        {
            if ((m_connection != null) && (m_connection.State == ConnectionState.Closed))
                m_connection.Open();
        }        

        #region Private Members

        private void Dispose ( bool disposing )
        {
            if (disposing)
            {
                try
                {
                    if ((m_connection != null) && (m_connection.State != ConnectionState.Closed))
                        m_connection.Close();
                } catch
                { /* Ignore */
                } finally
                {
                    m_connection = null;
                };
            };
        }
        
        private DbTransaction m_transaction;
        private DbConnection m_connection;

        #endregion
    }
}
