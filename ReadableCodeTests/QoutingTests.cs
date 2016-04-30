using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReadableCodeDomain;
using ReadableCodeRepositoryInterfaces;
using ReadableCodeServices;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Unity;

namespace ReadableCodeTests
{
    [TestClass]
    public class QoutingTests
    {
        class TestRunParams
        {
            public Func<decimal> Run;
            public decimal Expected;
            public string ScenarioDescription;
        }
        
        [TestClass]
        public class GetQuoteWithNoAccount : GetQuoteTestBase
        {
            IEnumerable<TestRunParams> _testScenarios;
            public GetQuoteWithNoAccount()
            {
                ItemRepository.SetupItems(new Item(1, 1), new Item(100,100), new Item(3,3), new Item(3,3));
                _testScenarios = new[] {
                    new TestRunParams
                    {
                        ScenarioDescription = "Given an item with price of 100, when quote is given, then the quoted cost should be 100.",
                        Run = () => GetQuote(100),
                        Expected = 100,
                    },
                    new TestRunParams
                    {
                        ScenarioDescription = "Given an item with prive of 1, when quote is given, then the quoted cost should be 1.",
                        Run = () => GetQuote(1),
                        Expected = 1,
                    },
                    new TestRunParams
                    {
                        ScenarioDescription = "Given an item with price of 1 and another item with a price of 100, when quote is given, then the quoted cost should be 101.",
                        Run = () => GetQuote(1, 100),
                        Expected = 101,
                    },
                    new TestRunParams
                    {
                        ScenarioDescription = "Given two of the same item with cost of 3, when a quote is given, then the quoted cost should be 6.",
                        Run = () => GetQuote(3),
                        Expected = 6,
                    }
                };
            }
            


            [TestMethod]
            public void RunGetQuoteWithNoAccountTestScenarios()
            {
                foreach (var scenario in _testScenarios)
                {
                    Assert.AreEqual(scenario.Expected, scenario.Run(), scenario.ScenarioDescription);
                }
            }


            [TestMethod]
            public void GivenItemPrice1_ShouldReturn1()
            {
                var quote = GetQuote(1);
                Assert.AreEqual(1, quote);
            }
                        
            [TestMethod]
            public void Given_multiple_items_gets_total_quote()
            {
                var actualTotalCost = GetQuote(1, 100);
                Assert.AreEqual(101.00M, actualTotalCost);
            }

            [TestMethod]
            public void Given_two_of_the_same_item_with_cost_3_total_should_be_six()
            {
                var actualTotalCost = GetQuote(3);
                Assert.AreEqual(6M, actualTotalCost);
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
                decimal quotedCostAfterDiscount = GetQuote(_itemId);
                Assert.AreEqual(90, quotedCostAfterDiscount);
            }

            [TestMethod]
            public void GivenAccountWithFlatDiscountOf10AndItemCost100_QuoteShouldBe90()
            {
                SetupFlatDiscount(10M);
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
                SetupNoDiscount();

                var quotedCostAfterDiscount = GetQuote(_itemId);

                ValidateQuotedCost(100, quotedCostAfterDiscount);
            }


            private void SetupPercentDiscount(decimal discountPercent)
            {
                AccountRespository.ReturnsAccountWithPercentDiscount(discountPercent);
            }

            private void SetupFlatDiscount(decimal discountAmount)
            {
                AccountRespository.ReturnsAccountWithFlatDiscount(discountAmount);
            }

            private void SetupNoDiscount()
            {
                AccountRespository.ReturnsAccountWithNoDiscount();
            }


            private void ValidateQuotedCostAfterDiscountIsEqualToExpectedCost(decimal expectedCostAfterDiscount)
            {
                decimal quotedCostAfterDiscount = GetQuote(_itemId);
                Assert.AreEqual(expectedCostAfterDiscount, quotedCostAfterDiscount);
            }
            private void ValidateQuotedCost(int expectedCost, decimal quotedCostAfterDiscount)
            {
                Assert.AreEqual(expectedCost, quotedCostAfterDiscount);
            }

            private decimal GetQuote(params int[] itemIds)
            {
                return _quotingService.GetQuote(new GetQuoteParameters { ItemIds = itemIds, AccountId = _accountId });
            }
        }
   }



    
    public abstract class GetQuoteTestBase
    {
        IUnityContainer _unityContainer = new UnityContainer();
        public GetQuoteTestBase()
        {
            _unityContainer
                .RegisterInstance(_itemRepository)
                .RegisterInstance(_accountRepository)
                .RegisterType<IQuotingService, QuotingService>();

            _quotingService = _unityContainer.Resolve<IQuotingService>();
        }
        private IRepository<Item> _itemRepository = new TestItemRepository();
        private IRepository<Account> _accountRepository = new TestAccountRepository();
        
        internal IQuotingService _quotingService;

        internal TestItemRepository ItemRepository { get { return _itemRepository as TestItemRepository; } }

        internal TestAccountRepository AccountRespository { get { return _accountRepository as TestAccountRepository; } }
        
    }

    internal class TestAccountRepository : IRepository<Account>
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
