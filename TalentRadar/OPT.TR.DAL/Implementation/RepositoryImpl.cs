using OPT.TalentRadar.DAL.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using System.Configuration;
using MongoDB.Bson;
using MongoDB.Driver.GridFS;
using System.IO;
using OPT.TalentRadar.Common.Data;
using System.Reflection;

namespace OPT.TalentRadar.DAL.Implementation
{
    public class RepositoryImpl : IRepository
    {
        private IMongoDatabase database;

        private string CollectionName;

        public RepositoryImpl(string collectionName)
        {
            this.CollectionName = collectionName;
            GetDatabase();
        }

        public ObjectId DocumentUpload<T>()
        {
            var fileName = @"D:\New folder\Kaushik\test.jpg";

            IGridFSBucket bucket = new GridFSBucket(database);
            using (var f = new FileStream(fileName, FileMode.Open))
            {
                var t = Task.Run<ObjectId>(() =>
                {
                    return bucket.UploadFromStreamAsync("Test.jpg", f);
                });

                t.Wait();
                var result = t.Result;
                DownloadFile(result);
                return result;
            }
        }

        public T IdGenerater<T>()
        {
            IMongoCollection<T> collection = GetCollection<T>();
            var fields = Builders<T>.Projection.Include("_id");
            var max = collection.Find(new BsonDocument()).Sort(Builders<T>.Sort.Descending("_id")).Limit(1).Project<T>(fields).FirstOrDefault();
            return max;
        }

        public List<T> FetchAll<T>()
        {
            IMongoCollection<T> collection = GetCollection<T>();
            var documents = collection.Find(new BsonDocument()).ToListAsync();
            documents.Wait();

            List<T> result = documents.Result.ToList<T>();
            return result;
        }

        public T Fetch<T>(string parameter, string token)
        {
            IMongoCollection<T> collection = GetCollection<T>();
            FilterDefinition<T> filter = null;
            if (parameter == "Token")
            {
                //ObjectId id = new ObjectId(token);
                filter = Builders<T>.Filter.Eq(parameter, token);
            }
            else
            {
                filter = Builders<T>.Filter.Regex(parameter, new BsonRegularExpression("/^" + token + "$/i")); //.Eq(parameter, token + "$/i");
            }
            var documents = collection.Find(filter).ToListAsync();
            documents.Wait();
            var result = documents.Result.FirstOrDefault();
            return result;
        }

        public T Fetch<T>(long id)
        {
            IMongoCollection<T> collection = GetCollection<T>();
            FilterDefinition<T> filter = Builders<T>.Filter.Eq("_id", id);
            var documents = collection.Find(filter).ToListAsync();
            documents.Wait();
            var result = documents.Result.FirstOrDefault();
            return result;
        }

        public void Save<T>(T obj)
        {
            IMongoCollection<T> collection = GetCollection<T>();
            var result = collection.InsertOneAsync(obj);
            result.Wait();
        }

        public void UpdateDocument<T>(FilterDefinition<T> query, T obj)
        {
            IMongoCollection<T> collection = GetCollection<T>();
            var result = collection.ReplaceOneAsync(query, obj);
            result.Wait();
        }

        public void SaveAll<T>(List<T> objs)
        {
            IMongoCollection<T> collection = GetCollection<T>();
            collection.InsertMany(objs);
        }

        public void DeleteAll<T>()
        {
            IMongoCollection<T> collection = GetCollection<T>();
            var result = collection.DeleteManyAsync(new BsonDocument());
            result.Wait();
        }

        public T DocumentsMatchEqFieldValue<T>(IDictionary<string, string> fieldEqValue)
        {
            IMongoCollection<T> collection = GetCollection<T>();
            var builder = Builders<T>.Filter;

            var filter = builder.Eq("Mobile", fieldEqValue["Mobile"]);
            filter = filter & builder.Eq("Email", fieldEqValue["Email"]);

            var documents = collection.Find(filter).ToListAsync();

            documents.Wait();

            T result = documents.Result.FirstOrDefault();

            return result;
        }

        public List<Candidate> DocumentsMatchFieldValue(CandidateDetails candidateDetails)
        {
            IMongoCollection<Candidate> collection = GetCollection<Candidate>();

            FilterDefinition<Candidate> filter = getFilterQuery(candidateDetails);
            List<Candidate> documents = null;
            if (filter != null)
            {
                documents = collection.Find(filter).ToList();
            }
            else {
                documents = collection.Find(new BsonDocument()).ToList();
            }
            return documents; 
        }

        #region Private
        private void GetDatabase()
        {
            IMongoClient client = new MongoClient(GetConnectionString());

            database = client.GetDatabase(GetDatabaseName());
        }

        private IMongoCollection<T> GetCollection<T>()
        {
            IMongoCollection<T> collection = database.GetCollection<T>(CollectionName);
            return collection;
        }

        private string GetDatabaseName()
        {
            return ConfigurationManager.AppSettings.Get("MongoDbDatabaseName");
        }

        private string GetConnectionString()
        {
            return ConfigurationManager.AppSettings.Get("MongoDbConnectionString").Replace("{DB_NAME}", GetDatabaseName());
        }

        private void DownloadFile(ObjectId id)
        {
            IGridFSBucket fs = new GridFSBucket(database);

            var x = fs.DownloadAsBytesAsync(id);
            x.Wait();
            byte[] a = x.Result;

            File.WriteAllBytes(@"d:\Foo.jpg", a);
        }

        private FilterDefinition<Candidate> getFilterQuery(CandidateDetails candidateDetails)
        {
            var builder = Builders<Candidate>.Filter;

            FilterDefinition<Candidate> filter = null;

            if (!string.IsNullOrWhiteSpace(candidateDetails.Mobile))
            {
                filter = builder.Eq(c => c.Mobile, candidateDetails.Mobile);
            }
            if (!string.IsNullOrWhiteSpace(candidateDetails.Email))
            {
                filter = filter != null ? filter & builder.Eq(c => c.Email, candidateDetails.Email) : builder.Eq(c => c.Email, candidateDetails.Email);
            }
            if (!string.IsNullOrWhiteSpace(candidateDetails.PracticeName))
            {
                filter = filter != null ? filter & builder.Eq(c => c.Practice.Name, candidateDetails.PracticeName) : builder.Eq(c => c.Practice.Name, candidateDetails.PracticeName);
            }
            if (!string.IsNullOrWhiteSpace(candidateDetails.PositionName))
            {
                filter = filter != null ? filter & builder.Eq(c => c.Position.Name, candidateDetails.PositionName) : builder.Eq(c => c.Position.Name, candidateDetails.PositionName);
            }
            if (!string.IsNullOrWhiteSpace(candidateDetails.InterviewStageName))
            {
                filter = filter != null ? filter & builder.Eq(c => c.Stage.Name, candidateDetails.InterviewStageName) : builder.Eq(c => c.Stage.Name, candidateDetails.InterviewStageName);
            }
            if (!string.IsNullOrWhiteSpace(candidateDetails.InterviewTypeName))
            {
                filter = filter != null ? filter & builder.Eq(c => c.InterviewType.Name, candidateDetails.InterviewTypeName) : builder.Eq(c => c.InterviewType.Name, candidateDetails.InterviewTypeName);
            }

            return filter;
        }
        #endregion
    }
}
