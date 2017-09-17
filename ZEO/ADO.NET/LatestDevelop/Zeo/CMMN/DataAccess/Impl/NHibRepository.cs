using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

using NHibernate;
using NHibernate.Linq;
using NHibernate.Criterion;
using NHibernate.Transform;

using MGI.Common.DataAccess.Contract;

using Spring.Stereotype;

namespace MGI.Common.DataAccess.Impl
{
	[Repository]
	public class NHibRepository<T> : IRepository<T>, IGuidKeyedRepository<T> where T : class
	{
		private ISessionFactory _sessionFactory;

		public ISessionFactory SessionFactory
		{
			protected get { return _sessionFactory; }
			set { _sessionFactory = value; }
		}

        protected ISession CurrentSession
        {
            get { return _sessionFactory.GetCurrentSession(); }
        }

	    #region IRepository<T> Members

		public object Add( T entity )
		{
			return CurrentSession.Save( entity );
		}

		public object AddWithFlush( T entity )
		{
			object obj = CurrentSession.Save( entity );
			CurrentSession.Flush();
			return obj;
		}
	 
	    public bool Add(System.Collections.Generic.IEnumerable<T> items)
	    {
	        foreach (T item in items)
	        {
	            CurrentSession.Save(item);
	        }
	        return true;
	    }
	 
	    public bool Update(T entity)
	    {
	        CurrentSession.Update(entity);
	        return true;
	    }

		public bool UpdateWithFlush( T entity )
		{
			CurrentSession.Update( entity );
			CurrentSession.Flush();
			return true;
		}

		public bool SaveOrUpdate( T entity )
		{
			CurrentSession.SaveOrUpdate( entity );
			return true;
		}
	 
	    public bool Delete(T entity)
	    {
	        CurrentSession.Delete(entity);
	        return true;
	    }
	 
	    public bool Delete(System.Collections.Generic.IEnumerable<T> entities)
	    {
	        foreach (T entity in entities)
	        {
	            CurrentSession.Delete(entity);
	        }
	        return true;
	    }

		public bool Merge( T entity )
		{
			CurrentSession.Merge( entity );
			return true;
		}

		public void Flush()
		{
			CurrentSession.Flush();
		}

	    #endregion
	 
	    #region IReadOnlyRepository<T> Members
	 
		public IQueryable<T> All()
		{
			return CurrentSession.Query<T>();
		}
	 
	    public T FindBy(System.Linq.Expressions.Expression<System.Func<T, bool>> expression)
	    {
	        return FilterBy(expression).SingleOrDefault();
	    }

		public T FindByChildCriteria<C>( string childGroupName, string childParamName, object searchValue )
		{
			string childTableAlias = childGroupName.Substring( 0, 1 ).ToLower();
			return CurrentSession.CreateCriteria<T>()
				.CreateAlias( childGroupName, childTableAlias, NHibernate.SqlCommand.JoinType.InnerJoin )
				.Add( Restrictions.Eq( string.Format("{0}.{1}",childTableAlias, childParamName), searchValue ) )
				.SetResultTransformer( Transformers.DistinctRootEntity ).UniqueResult<T>();
		}

		public List<T> FilterByChildCriteria( string childGroupName, string childParamName, object searchValue )
		{
			string childTableAlias = childGroupName.Substring( 0, 1 ).ToLower();
			return CurrentSession.CreateCriteria<T>()
				.CreateAlias( childGroupName, childTableAlias, NHibernate.SqlCommand.JoinType.InnerJoin )
				.Add( Restrictions.Eq( string.Format( "{0}.{1}", childTableAlias, childParamName ), searchValue ) )
				.SetResultTransformer(Transformers.RootEntity).List<T>().ToList<T>();
		}

	    public IQueryable<T> FilterBy(System.Linq.Expressions.Expression<System.Func<T, bool>> expression)
	    {
	        return All().Where(expression).AsQueryable();
	    }
	 
	    #endregion

		#region IGuidKeyedRepository<T> Members

		public T FindBy( Guid id )
		{
			return CurrentSession.Get<T>( id );
		}

		#endregion
	}
}
