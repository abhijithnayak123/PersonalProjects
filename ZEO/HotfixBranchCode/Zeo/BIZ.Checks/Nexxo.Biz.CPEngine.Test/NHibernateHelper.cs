using System;
using System.Collections.Generic;
using System.Reflection;

using NHibernate;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;

using System.IO;

namespace MGI.Biz.MoneyTransfer.Test
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static Configuration CreateConfiguration()
        {
            var configuration = new Configuration();
            string configPath = AppDomain.CurrentDomain.BaseDirectory;
            configuration.Configure(configPath.Substring(0, configPath.IndexOf("bin")) + "hibernate.cfg.xml");

            //Loads nhibernate mappings 
            configuration.AddAssembly("MGI.Cxn.MoneyTransfer.Data");
            configuration.AddAssembly("MGI.Cxn.MoneyTransfer.WU.Data");
            configuration.AddAssembly("Nexxo.Common.Logging.Data");
            return configuration;
        }
    }
}
