using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossCuttingConcerns.Caching
{
    public interface ICache
    {
        void Add(string key, object item, TimeSpan timeSpan);

        void Remove(string key);

        object Get(string key);
    }
}
