using ReadableCodeDomain;
using ReadableCodeRepositoryInterfaces;
using System;
using System.Collections.Generic;
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
            //should the quote service have the responsibility to fetch everything needed for the quote?
            //what if it took in all the parameters instead.
            //If everything truly is a service (repositories included) there would be tradeoffs between chattiness and message size
            //Also, the quoting service would be coupled to the account repository and the item repository.
            //Rather than this, we could couple them to a data contract / data transfer object (GetQuoteParameters) and gain some reusability.
            //what does quoting really  need in order to perform it's function?
            //One specific concern is rehydrating an account object. This object would be shared throughout the system.
            //Rather than synching the object, it might be preferrable to consider using an account service to apply the discount. In this way
            //the discounting would be centralized and the implementations of the account wouldn't need to be distributed amongst other processes.



            //This would not not work if we are quoting for shipping, 
            //typically an entire shipment would be quoted according to destination, time, and size parameters.
            //Would likely send in a shipment to get the quote.
            var items = _itemRepository.GetMany(inputParams.ItemIds);


            //How do we apply a one-time discount? Maybe the account has credits to apply, 
            //or some other specific one-time discount that they wish to use once.
            return ApplyAccountDiscount(items, inputParams.AccountId)
                .Select(item => item.Cost)
                .Sum();
        }

        private IEnumerable<Item> ApplyAccountDiscount(IEnumerable<Item> items, int? accountId)
        {
            //SMELL:should this really be in the quoting service? should it be done at all?
            //Maybe users need an account to quote, even if they have an account with no discount. 
            var account = accountId.HasValue ? 
                _accountRepository.Get(accountId.Value) : 
                //default account discount is 0, used as a proxy account for quotes with no account so that logic is cleaner.
                new DefaultAccount();
            //SMELL:if ApplyDisount has rules based on Items or total cost of several items, this will not work well.items
            return items.Select(item => new Item(item.Id, account.ApplyDiscount(item.Cost)));
        }
    }



    public interface IQuotingService
    {
        decimal GetQuote(GetQuoteParameters inputParams);
    }
}
