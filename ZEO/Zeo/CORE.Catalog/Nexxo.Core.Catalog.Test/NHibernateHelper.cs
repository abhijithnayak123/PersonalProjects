﻿using System;
using System.Collections.Generic;
using System.Reflection;

using NHibernate;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;



namespace MGI.Core.Catalog.Test
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

        public static ISession Session
        {
            get
            {
                //Open and return the nhibernate session
                return SessionFactory.OpenSession();
            }
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
            //var configuration = new Configuration();
            ////Loads properties from hibernate.cfg.xml
            //configuration.Configure();
            
            var configuration = new Configuration();
            string configPath = AppDomain.CurrentDomain.BaseDirectory;
            configuration.Configure(configPath.Substring(0, configPath.IndexOf("bin")) + "hibernate.cfg.xml");

			//Loads nhibernate mappings 
			//configuration.AddAssembly(Assembly.GetAssembly(typeof(CustomerProfile)));
          
            configuration.AddAssembly("MGI.Core.Catalog.Data");           
			return configuration;
		}
	}
}
