using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Common.DataAccess.Contract
{
    public interface IGuidKeyedRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Query to Get entity based on unique identifier. 
        /// </summary>
        /// <param name="id">The unique identifier for entity</param>
        /// <returns>entity details</returns>
        TEntity FindBy(Guid id);
    }
}
