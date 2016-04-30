using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnReadableCodeDataModel;

namespace ReadableCodeRepository
{
    internal class DataAccess
    {

        ShippingDataModel _dataContext = new ShippingDataModel();

        internal Item GetItem(int id)
        {
            return _dataContext.ITEMS
                .FirstOrDefault(x => x.Id == id);
        }

        internal Item[] GetItems(IEnumerable<int> itemIds)
        {
            return _dataContext.ITEMS
                .Where(item => itemIds.Contains(item.Id))
                .ToArray();
        }

        internal Account GetAccount(int accountId)
        {
            return _dataContext.Accounts.FirstOrDefault(x => x.accountId == accountId);
        }
    }
}
