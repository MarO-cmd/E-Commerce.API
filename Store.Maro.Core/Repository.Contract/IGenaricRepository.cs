using Store.Maro.Core.Entities;
using Store.Maro.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Maro.Core.Repository.Contract
{
    public interface IGenaricRepository<TEntity,TKey> where TEntity: BaseEntity<TKey>
    {
        public Task<IEnumerable<TEntity>> GetAllAsync();
        public Task<TEntity> GetByIdAsync(TKey id);
        public Task<IEnumerable<TEntity>> GetAllWithSpecAsync(ISpecifications<TEntity,TKey> spec);
        public Task<TEntity> GetWithSpecAsync(ISpecifications<TEntity, TKey> spec);
        public Task<int> GetCountAsync(ISpecifications<TEntity, TKey> spec);
        public Task AddAsync(TEntity entity);
        public void Update(TEntity entity);
        public void Delete(TEntity entity);



    }
}
