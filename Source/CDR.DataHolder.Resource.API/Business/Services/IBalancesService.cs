using System;
using System.Threading.Tasks;
using CDR.DataHolder.Resource.API.Business.Responses;

namespace CDR.DataHolder.Resource.API.Business.Services
{
    public interface IBalancesService
    {
        Task<ResponseBankingAccountsBalanceById> GetAccountBalance(string accountId, Guid customerId);
    }
}
