using System;
using System.Threading.Tasks;
using CDR.DataHolder.Domain.Repositories;
using CDR.DataHolder.Resource.API.Business.Responses;

namespace CDR.DataHolder.Resource.API.Business.Services
{
    public class BalancesService: IBalancesService
    {
        private readonly IResourceRepository _resourceRepository;
        private readonly AutoMapper.IMapper _mapper;

        public BalancesService(IResourceRepository resourceRepository, AutoMapper.IMapper mapper)
        {
            _resourceRepository = resourceRepository;
            _mapper = mapper;
        }

        public async Task<ResponseBankingAccountsBalanceById> GetAccountBalance(string accountId, Guid customerId)
        {
            var results = await _resourceRepository.GetAccountBalanceByAccountId(accountId, customerId);
            return _mapper.Map<ResponseBankingAccountsBalanceById>(results);
        }
    }
}
