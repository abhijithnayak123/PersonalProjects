using System;
using System.Collections.Generic;

namespace MGI.Common.DataAccess.Contract
{
    public interface IRepository<TEntity> : IReadOnlyRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Persist the given transient instance, first assigning a generated identifier.
        /// </summary>
        /// <param name="entity">A transient instance of a persistent class</param>
        /// <returns>The generated identifier(primary key id)</returns>
        object Add(TEntity entity);

        /// <summary>
        /// Persist the given transient instance, first assigning a generated identifier. Force the ISession to flush.
        /// </summary>
        /// <param name="entity">A transient instance of a persistent class</param>
        /// <returns>The generated identifier(primary key id)</returns>
        object AddWithFlush(TEntity entity);

        /// <summary>
        /// Persist the given transient instance, first assigning a generated identifier.
        /// </summary>
        /// <param name="entity">A transient instance of a persistent class</param>
        /// <returns>Query Execution Status</returns>
        bool Add(IEnumerable<TEntity> items);

        /// <summary>
        /// Update the persistent instance with the identifier of the given transient instance. 
        /// </summary>
        /// <param name="entity">A transient instance containing updated state</param>
        /// <returns>Query Execution Status</returns>
        bool Update(TEntity entity);

        /// <summary>
        /// Update the persistent instance with the identifier of the given transient instance. Force the ISession to flush.
        /// </summary>
        /// <param name="entity">A transient instance containing updated state</param>
        /// <returns>Query Execution Status</returns>
        bool UpdateWithFlush(TEntity entity);

        /// <summary>
        /// Copy the state of the given object onto the persistent object with the same
        /// identifier. If there is no persistent instance currently associated with
        /// the session, it will be loaded. Return the persistent instance. If the given
        /// instance is unsaved, save a copy of and return it as a newly persistent instance.
        /// The given instance does not become associated with the session.  This operation
        /// cascades to associated instances if the association is mapped with cascade="merge".
        /// The semantics of this method are defined by JSR-220.
        /// </summary>
        /// <param name="entity">a detached instance with state to be copied</param>
        /// <returns>Query Execution Status</returns>
        bool Merge(TEntity entity);

        /// <summary>
        /// Either Save() or Update() the given instance, depending upon the value of its identifier property.
        /// </summary>
        /// <param name="entity">A transient instance containing new or updated state</param>
        /// <returns>Query Execution Status</returns>
        bool SaveOrUpdate(TEntity entity);

        /// <summary>
        /// Remove a persistent instance from the datastore.
        /// </summary>
        /// <param name="entity">The instance to be removed</param>
        /// <returns>Query Execution Status</returns>
        bool Delete(TEntity entity);

        /// <summary>
        /// Remove collection of a persistent instance from the datastore.
        /// </summary>
        /// <param name="entities">The instance of collection to be removed</param>
        /// <returns>Query Execution Status</returns>
        bool Delete(IEnumerable<TEntity> entities);

        /// <summary>
        /// Force the ISession to flush.
        /// </summary>
        void Flush();
    }
}
