using MongoDB.Bson;
using util.API.Models;
using util.API.Models.Request;
using util.API.Repository;

namespace util.API.Service
{
    public interface ILedgerService
    {
        Task TestInsertData(List<LedgerModel> request);
        Task<List<LedgerModel>> GetLedgerAsync(string username);
        Task UpsertLedgerAsync(string username, InsertLedgerRequest request);
    }
    public class LedgerService : ILedgerService
    {
        private readonly ILedgerRepository _ledgerRepo;
        public LedgerService(ILedgerRepository ledgerRepo)
        {
            _ledgerRepo = ledgerRepo;
        }

        public async Task<List<LedgerModel>> GetLedgerAsync(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentNullException(nameof(username));
            }

            var ledger = await _ledgerRepo.GetLedgerAsync(username);
            return ledger;
        }

        public async Task UpsertLedgerAsync(string username, InsertLedgerRequest request)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentNullException(nameof(username));
            }

            var ledger = await _ledgerRepo.GetLedgerByDateAsync(username, request.LedgerDate);
            if (ledger is not null) // update
            {
                var newDetail = new List<LedgerDetail>();
                newDetail.AddRange(ledger.LedgerDetails);
                newDetail.Add(new LedgerDetail()
                {
                    LedgerType = request.LedgerType,
                    LedgerNote = request.LedgerNote,
                    LedgerValue = request.LedgerValue
                });

                await _ledgerRepo.UpdateLedgerAsync(ledger.Id, newDetail);
            }
            else // insert
            {
                var newLedger = new LedgerModel()
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    UserName = username,
                    LedgerDate = request.LedgerDate,
                    LedgerDetails = new List<LedgerDetail>()
                    {
                        new LedgerDetail() 
                        {
                            LedgerType = request.LedgerType,
                            LedgerNote = request.LedgerNote,
                            LedgerValue = request.LedgerValue
                        }
                    }
                };
                await _ledgerRepo.InsertLedgerAsync(newLedger);
            }
        }

        public async Task TestInsertData(List<LedgerModel> request)
        {
            await _ledgerRepo.TestInsert(request);
        }
    }
}
