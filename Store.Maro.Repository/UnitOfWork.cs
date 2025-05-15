using Store.Maro.Core;
using Store.Maro.Core.Entities;
using Store.Maro.Core.Repository.Contract;
using Store.Maro.Repository.Data.Context;
using Store.Maro.Repository.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Maro.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        Hashtable _repositories;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            _repositories = new Hashtable();
        }
        public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();
        

        public IGenaricRepository<TEntity, TKey> Repository<TEntity, TKey>() where TEntity : BaseEntity<TKey>
        {
            var type = typeof(TEntity).Name;

            // cuz not every time i ask for an object create a new one 
            if (! _repositories.ContainsKey(type))
            { 
                var repo = new GenaricRepository<TEntity,TKey>(_context);
                _repositories.Add(type,repo);
            }

            return (IGenaricRepository<TEntity, TKey>) _repositories[type];
        }
    }
}
