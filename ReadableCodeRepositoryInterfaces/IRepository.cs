using System;
using System.Collections.Generic;

namespace ReadableCodeRepositoryInterfaces
{
    public interface IRepository<T>
    {
        T Get(int id);
        IEnumerable<T> GetMany(IEnumerable<int> itemIds);
    }
}