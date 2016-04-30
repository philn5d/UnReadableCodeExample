using System;
using System.Collections.Generic;
using ReadableCodeDomain;
using ReadableCodeRepositoryInterfaces;

namespace ReadableCodeRepository
{
    public class AccountRepository : IRepository<Account>
    {
        DataAccess dataAccess = new DataAccess();
        Mapper Mapper = new Mapper();

        public Account Get(int id)
        {
            return Mapper.Map<Account>(dataAccess.GetAccount(id));
        }

        public IEnumerable<Account> GetMany(IEnumerable<int> itemIds)
        {
            throw new NotImplementedException();
        }
    }
}