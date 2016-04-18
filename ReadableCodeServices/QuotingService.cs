using ReadableCodeDomain;
using ReadableCodeRepositoryInterfaces;
using System;
using System.Linq;

namespace ReadableCodeServices
{

    public class QuotingService : IQuotingService
    {
        private IRepository<Account> _accountRepository;
        private IRepository<Item> _itemRepository;

        public QuotingService(IRepository<Item> itemRepository, IRepository<Account> accountRepository)
        {
            _itemRepository = itemRepository;
            _accountRepository = accountRepository;
        }

        public decimal GetQuote(GetQuoteParameters inputParams)
        {
            var account = inputParams.AccountId.HasValue ? 
                _accountRepository.Get(inputParams.AccountId.Value) : 
                //default account discount is 0, used as a proxy account for quotes with no account so that logic is cleaner.
                new DefaultAccount();

            var items = _itemRepository.GetMany(inputParams.ItemIds);

            return items
                .Select(item => GetItemCost(item, account))
                .Sum();
        }

        private decimal GetItemCost(Item item, Account account)
        {
            var costWithDiscount = account.ApplyDiscount(item.Cost);
            return costWithDiscount;
        }
    }



    public interface IQuotingService
    {
        decimal GetQuote(GetQuoteParameters inputParams);
    }
}
