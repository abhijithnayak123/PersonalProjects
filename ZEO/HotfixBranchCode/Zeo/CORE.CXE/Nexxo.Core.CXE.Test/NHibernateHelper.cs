using System;
using System.Collections.Generic;
using System.Reflection;

using NHibernate;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;

using MGI.Core.CXE.Data;

namespace MGI.Core.CXE.Test
{
	public static class NHibernateHelper
	{
		private static ISessionFactory _sessionFactory;
		private static Configuration _configuration;
		//private static HbmMapping _mapping;

		public static ISession OpenSession()
		{
			//Open and return the nhibernate session
			return SessionFactory.OpenSession();
		}

		public static ISessionFactory SessionFactory
		{
			get
			{
				if ( _sessionFactory == null )
				{
					//Create the session factory
					_sessionFactory = Configuration.BuildSessionFactory();
				}
				return _sessionFactory;
			}
		}

		public static Configuration Configuration
		{
			get
			{
				if ( _configuration == null )
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
			//Loads properties from hibernate.cfg.xml
			configuration.Configure();
			//Loads nhibernate mappings 
			configuration.AddAssembly( Assembly.GetAssembly(typeof(CustomerProfile)));
			return configuration;
		}
	}
}
