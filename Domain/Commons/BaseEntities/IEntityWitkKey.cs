using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Commons.BaseEntities
{
    public interface IEntityWitkKey<TKey> 
    {
        TKey Id { get; set; }
    }
}
