using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace MGI.Common.DataAccess.Contract
{
    public interface IReadOnlyRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Query to get persistent entity collection.
        /// </summary>
        /// <returns>List Of entity Details</returns>
        IQueryable<TEntity> All();

        /// <summary>
        /// Query to find the persistent entity object based on expression.
        /// </summary>
        /// <param name="expression">strongly typed lambda expression</param>
        /// <returns>entity Details</returns>
        TEntity FindBy(Expression<Func<TEntity, bool>> expression);

        /// <summary>
        /// Query to filter the persistent entity object based on expression.
        /// </summary>
        /// <param name="expression">strongly typed lambda expression</param>
        /// <returns>List Of entity Details</returns>
        IQueryable<TEntity> FilterBy(Expression<Func<TEntity, bool>> expression);

        /// <summary>
        /// Query for a parent based on a child's criteria
        /// </summary>
        /// <typeparam name="ChildEntity">Child entity type</typeparam>
        /// <param name="childGroupName">The name of the child collection in the parent</param>
        /// <param name="childParamName">The name of the child parameter to search on</param>
        /// <param name="searchValue">The value to search for</param>
        /// <returns>entity Details</returns>
        TEntity FindByChildCriteria<ChildEntity>(string childGroupName, string childParamName, object searchValue);

        /// <summary>
        /// Query for a filter parent based on child's criteria.
        /// </summary>
        /// <param name="childGroupName">The name of the child collection in the parent</param>
        /// <param name="childParamName">The name of the child parameter to search on</param>
        /// <param name="searchValue">The value to search for</param>
        /// <returns>List Of entity Details</returns>
        List<TEntity> FilterByChildCriteria(string childGroupName, string childParamName, object searchValue);
    }
}
