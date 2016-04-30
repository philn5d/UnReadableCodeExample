using Microsoft.Practices.Unity;
using ReadableCodeServices;
using ReadableCodeRepositoryInterfaces;
using ReadableCodeDomain;
using ReadableCodeRepository;
using System;
using System.Collections.Generic;

namespace QuotingWindowsApplication
{
    internal static class UnityContainerHelper
    {
        static IUnityContainer _container;
        static UnityContainerHelper()
        {
            _container = new UnityContainer();
        }

        internal static void Configure()
        {
            _container
                .RegisterType<IRepository<Item>, ItemRespository>()
                .RegisterType<IRepository<Account>, AccountRepository>()
                .RegisterType<IQuotingService, QuotingService>();

        }

        internal static T GetInstance<T>()
        {
            return _container.Resolve<T>();
        }
    }

    internal class FakeAccountRepository : IRepository<Account>
    {
        public Account Get(int id)
        {
            Account account = null;
            switch(id)
            {
                case 1:
                    account = new Account
                    {
                        Discount = new PercentDiscount(0.10M)
                    };
                    break;
                case 2:
                    account = new Account
                    {
                        Discount = new FlatDiscount(5M)
                    };
                    break;
                default:
                    account = new Account();
                    break;
            }
            return account;
     
        }

        public IEnumerable<Account> GetMany(IEnumerable<int> itemIds)
        {
            throw new NotImplementedException();
        }
    }
}
