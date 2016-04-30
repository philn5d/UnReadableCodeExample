using System.Collections.Generic;
using System.Linq;

namespace ReadableCodeRepository
{
    internal class Mapper
    {
        internal ReadableCodeDomain.Item Map<T>(UnReadableCodeDataModel.Item item) where T : ReadableCodeDomain.Item
        {
            if(item == null)
            {
                return null;
            }
            return new ReadableCodeDomain.Item(item.Id, 1);
        }

        internal ReadableCodeDomain.Item[] Map<T>(IEnumerable<UnReadableCodeDataModel.Item> items) where T : IEnumerable<ReadableCodeDomain.Item>
        {
            if (items == null)
            {
                return null;
            }
            return items.Select(item => Map<ReadableCodeDomain.Item>(item)).ToArray();
            
        }

        internal ReadableCodeDomain.Account Map<T>(UnReadableCodeDataModel.Account account) where T : ReadableCodeDomain.Account
        {
            if(account == null)
            {
                return null;
            }
            return new ReadableCodeDomain.Account
            {
            };
        }
    }
}