using NHibernate;
using NHibernate.Cfg;

namespace MGI.Cxn.MoneyTransfer.WU.Test
{
	public static class NHibernateHelper
	{
		private static ISessionFactory _sessionFactory;
		private static Configuration _configuration;

		public static ISession OpenSession()
		{
			//Open and return the nhibernate session
			return SessionFactory.OpenSession();
		}

		public static ISession Session
		{
			get
			{
				//Open and return the nhibernate session
				return SessionFactory.OpenSession();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public static ISessionFactory SessionFactory
		{
			get
			{
				if (_sessionFactory == null)
				{
					//Create the session factory
					_sessionFactory = Configuration.BuildSessionFactory();
				}
				return _sessionFactory;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public static Configuration Configuration
		{
			get
			{
				if (_configuration == null)
				{
					//Create the nhibernate configuration
					_configuration = CreateConfiguration();
				}
				return _configuration;
			}
		}

		private static Configuration CreateConfiguration()
		{
			var configuration = new Configuration();
			configuration.Configure();

			//Loads nhibernate mappings 
            configuration.AddAssembly("MGI.Cxn.MoneyTransfer.Data");
            configuration.AddAssembly("MGI.Cxn.MoneyTransfer.WU.Data");
			configuration.AddAssembly("Nexxo.Common.Logging.Data");
			
			return configuration;
		}
	}
}
