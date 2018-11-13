using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MGI.Common.TransactionalLogging.Contract;
using MGI.Common.TransactionalLogging.Data;
using System.Configuration;
using MongoDB.Driver;

namespace MGI.Common.TransactionalLogging.Impl
{
    public class TransactionLogImpl : ITLogger
    {
        //IMongoDatabase database;
        //private IMongoCollection<TLogEntry> collection;
        MongoDatabase database;
        private MongoCollection<TransactionLogEntry> collection;

        public TransactionLogImpl()
        {
            GetDatabase();
            GetCollection();
        }

        private void GetDatabase()
        {
            var client = new MongoClient(GetConnectionString());

            var server = client.GetServer();
            
             database = server.GetDatabase(GetDatabaseName());
            
        }

        private void GetCollection()
        {
			collection = database.GetCollection<TransactionLogEntry>(ConfigurationManager.AppSettings.Get("CollectionName"));
        }

        private string GetDatabaseName()
        {
            return ConfigurationManager.AppSettings.Get("MongoDbDatabaseName");
        }

        private string GetConnectionString()
        {
            return ConfigurationManager.AppSettings.Get("MongoDbConnectionString").Replace("{DB_NAME}", GetDatabaseName());
        }


        /// <summary>
        /// Savelogs the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        public void Savelog(TransactionLogEntry obj)
        {

            try
            {
               
                collection.Insert(obj);
                // var collection = database.GetCollection<TLogEntry>("testLogs");
                // .NET Framework 4.5 code with async await
                                 //await collection.InsertOneAsync(obj);
                //var list = await collection.Find(x => x.FunctionName == "Test fail").ToListAsync();

                //foreach (var excp in list)
                //{
                //    Console.WriteLine(excp.FunctionName);
                //}
				
                ///  Task.Factory.StartNew(() => collection.Insert(obj));                 
                ////var collections = collection.Insert(obj);
                //Task _taskCollection = new Task(collection.Insert(obj));
                //_taskCollection.Wait();
            }

            catch (Exception ex)
            {
				throw ex;
            }

        }

        /// <summary>
        /// Readlogs this instance.
        /// </summary>
        /// <returns></returns>
        public TransactionLogEntry ReadSinglelog()
        {

			TransactionLogEntry result = collection.FindOne(); //.ToListAsync();
            
            return result;
        }

        public List<TransactionLogEntry> Readlog()
        {

            List<TransactionLogEntry> result = collection.FindAll().ToList(); //.ToListAsync();

            return result;
        }
    }
}
