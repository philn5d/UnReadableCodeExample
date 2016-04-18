using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReadableCodeDomain;
using ReadableCodeRepositoryInterfaces;
using ReadableCodeServices;
using System.Collections.Generic;
using System.Linq;

namespace ReadableCodeTests
{
    [TestClass]
    public class QoutingTests
    {
        [TestClass]
        public class GetQuoteWithNoAccount : GetQuoteTestBase
        {
            
            public GetQuoteWithNoAccount()
            {
                ItemRepository.SetupItems(new Item(1, 1), new Item(2, 1), new Item(100,100));
            }

            [TestMethod]
            public void GivenItemPrice100_ShouldReturn100()
            {
                var quote = GetQuote(100);
                Assert.IsTrue(quote == 100);
            }


            [TestMethod]
            public void GivenItemPrice1_ShouldReturn1()
            {
                var quote = GetQuote(1);
                Assert.IsTrue(quote == 1);

            }
                        
            [TestMethod]
            public void Given_multiple_items_gets_total_quote()
            {
                var actualTotalCost = GetQuote(1, 2);
                Assert.AreEqual(2.00M, actualTotalCost);
            }


            private decimal GetQuote(params int[] itemIds)
            {
                return _quotingService.GetQuote(new GetQuoteParameters { ItemIds = itemIds });
            }
        }

        [TestClass]
        public class GetQuoteWithAccount : GetQuoteTestBase
        {
            int _itemId, _accountId;

            public GetQuoteWithAccount()
            {
                _itemId = 1;
                _accountId = 1;
                ItemRepository.ReturnsItemWithCost(100);
            }

            [TestMethod]
            public void GivenAccountWith10PercentDiscountAndItemCost100_QuoteShouldBe90()
            {
                AccountRespository.ReturnsAccountWithPercentDiscount(.10M);
                ValidateQuotedCostAfterDiscountIsEqualToExpectedCost(90);
            }


            [TestMethod]
            public void GivenAccountWithFlatDiscountOf10AndItemCost100_QuoteShouldBe90()
            {
                AccountRespository.ReturnsAccountWithFlatDiscount(10.00M);
                ValidateQuotedCostAfterDiscountIsEqualToExpectedCost(90);
            }

            [TestMethod]
            public void GivenAccountWithFlatDiscountOf101AndItemCost100_QuoteShouldBe0()
            {
                AccountRespository.ReturnsAccountWithFlatDiscount(101.00M);
                ValidateQuotedCostAfterDiscountIsEqualToExpectedCost(0);
            }

            [TestMethod]
            public void GivenAccountWithNoDiscount_AndItemCost100_QuioteShouldBe100()
            {
                AccountRespository.ReturnsAccountWithNoDiscount();
                ValidateQuotedCostAfterDiscountIsEqualToExpectedCost(100);
            }
            
            private void ValidateQuotedCostAfterDiscountIsEqualToExpectedCost(decimal expectedCostAfterDiscount)
            {
                decimal quotedCostAfterDiscount = GetQuote(_itemId);
                Assert.AreEqual(expectedCostAfterDiscount, quotedCostAfterDiscount);
            }

            private decimal GetQuote(params int[] itemIds)
            {
                return _quotingService.GetQuote(new GetQuoteParameters { ItemIds = itemIds, AccountId = _accountId });
            }
        }
   }




    public abstract class GetQuoteTestBase
    {
        private IRepository<Item> _itemRepository = new TestItemRepository();
        private IRepository<Account> _accountRepository = new TestAccountRespository();
        
        internal IQuotingService _quotingService;

        internal TestItemRepository ItemRepository { get { return _itemRepository as TestItemRepository; } }

        internal TestAccountRespository AccountRespository { get { return _accountRepository as TestAccountRespository; } }

        [TestInitialize]
        public void Init()
        {
            _quotingService = new QuotingService(_itemRepository, _accountRepository);
        }
    }

    internal class TestAccountRespository : IRepository<Account>
    {
        Account _account;

        public Account Get(int id)
        {
            return _account;
        }

        public IEnumerable<Account> GetMany(IEnumerable<int> itemIds)
        {
            throw new NotImplementedException();
        }

        internal void ReturnsAccountWithFlatDiscount(decimal flatDiscount)
        {
            _account = new Account();
            _account.Discount = new FlatDiscount(flatDiscount);
        }

        internal void ReturnsAccountWithNoDiscount()
        {
            _account = new Account();
        }

        internal void ReturnsAccountWithPercentDiscount(decimal percentDiscount)
        {
            _account = new Account();
            _account.Discount = new PercentDiscount(percentDiscount);
        }
    }

    internal class TestItemRepository : IRepository<Item>
    {
        List<Item> _items = new List<Item>();

        public Item Get(int id)
        {
            return _items.Find(x => x.Id == id);
        }

        internal void SetupItems(params Item[] items)
        {
            _items.Clear();
            _items.AddRange(items);
        }

        internal void ReturnsItemWithCost(decimal itemCost)
        {
            _items.Clear();
            var item = new Item(1, itemCost);
            _items.Add(item);
        }

        public IEnumerable<Item> GetMany(IEnumerable<int> itemIds)
        {
            return _items.Where(x => itemIds.Contains(x.Id));
        }
    }
}
