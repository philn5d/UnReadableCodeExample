using ReadableCodeDomain;
using ReadableCodeRepositoryInterfaces;
using System.Collections.Generic;

namespace ReadableCodeRepository
{
    public class ItemRespository : IRepository<Item>
    {
        DataAccess dataAccess = new DataAccess();
        Mapper Mapper = new Mapper();
        
        public Item Get(int id)
        {
            return Mapper.Map<Item>(dataAccess.GetItem(id));
        }

        public IEnumerable<Item> GetMany(IEnumerable<int> itemIds)
        {
            return Mapper.Map<Item[]>(dataAccess.GetItems(itemIds));
        }
    }
}