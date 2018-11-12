using MongoDB.Bson;
using MongoDB.Driver;
using OPT.TalentRadar.Common.Data;
using System.Collections.Generic;

namespace OPT.TalentRadar.DAL.Contract
{
    public interface IRepository
    {
        void Save<T>(T obj);

        T Fetch<T>(long id);

        T Fetch<T>(string parameter, string token);

        List<T> FetchAll<T>();

        void UpdateDocument<T>(FilterDefinition<T> query, T obj);

        T IdGenerater<T>();

        void SaveAll<T>(List<T> obj);

        void DeleteAll<T>();

        ObjectId DocumentUpload<T>();

        T DocumentsMatchEqFieldValue<T>(IDictionary<string, string> fieldEqValue);

        List<Candidate> DocumentsMatchFieldValue(CandidateDetails candidateDetails);
    }
}
