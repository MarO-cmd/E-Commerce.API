using Store.Maro.Core.Entities;
using Store.Maro.Core.Repository.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Maro.Core
{
    public interface IUnitOfWork 
    {
        Task<int> CompleteAsync();
        
        IGenaricRepository<TEntity,TKey> Repository<TEntity , TKey>() where TEntity : BaseEntity<TKey>;
    }
}
