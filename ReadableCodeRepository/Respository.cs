using ReadableCodeRepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ReadableCodeRepository
{
    internal class Respository<T> : IRepository<T> where T : new()
    {
        public T Get(int id)
        {
            return new T();
        }

        public IEnumerable<T> GetMany(IEnumerable<int> itemIds)
        {
            return itemIds.Select(id => new T());
        }
    }
}