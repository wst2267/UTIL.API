using Microsoft.Extensions.Options;
using MongoDB.Driver;
using util.API.Models;
using util.API.Utility.Configuration;

namespace util.API.Repository
{
    public interface ILedgerRepository
    {
        Task TestInsert(List<LedgerModel> request);
        Task<List<LedgerModel>> GetLedgerAsync(string username);
        Task<LedgerModel> GetLedgerByDateAsync(string username, string ledgerDate);
        Task InsertLedgerAsync(LedgerModel request);
        Task UpdateLedgerAsync(string id, List<LedgerDetail> newDetail);
    }
    public class LedgerRepository : ILedgerRepository
    {
        private readonly IMongoCollection<LedgerModel> _ledgerCollection;
        public LedgerRepository (IOptions<MongoDbConfig> dbConfig)
        {
            var connection = new MongoClient(dbConfig.Value.ConnectionString);
            var db = connection.GetDatabase(dbConfig.Value.DatabaseName);
            _ledgerCollection = db.GetCollection<LedgerModel>(dbConfig.Value.LedgerCollection);
        }

        public async Task<List<LedgerModel>> GetLedgerAsync(string username)
        {
            var ledger = await _ledgerCollection.Find(x => x.UserName == username).ToListAsync();
            return ledger;
        }

        public async Task<LedgerModel> GetLedgerByDateAsync(string username, string ledgerDate)
        {
            var ledger = await _ledgerCollection.Find(x => x.UserName == username && x.LedgerDate == ledgerDate).FirstOrDefaultAsync();
            return ledger;
        }

        public async Task TestInsert(List<LedgerModel> request)
        {
            await _ledgerCollection.InsertManyAsync(request);
        }

        public async Task InsertLedgerAsync(LedgerModel request)
        {
            await _ledgerCollection.InsertOneAsync(request);
        }

        public async Task UpdateLedgerAsync(string id, List<LedgerDetail> newDetail)
        {
            var filter = Builders<LedgerModel>.Filter.Eq(s => s.Id, id);
            var update = Builders<LedgerModel>.Update.Set(s => s.LedgerDetails, newDetail);
            var result = await _ledgerCollection.UpdateOneAsync(filter, update);
        }
    }
}
