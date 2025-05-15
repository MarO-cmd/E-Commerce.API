﻿using Store.Maro.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Store.Maro.Core.Specifications
{
    public class BaseSpecifications<TEntity,TKey> : ISpecifications<TEntity,TKey>  where TEntity: BaseEntity<TKey>
    {
        // where(.....)
        public Expression<Func<TEntity, bool>> Criteria { get; set; } = null;
        public List<Expression<Func<TEntity, object>>> Includes { get; set; } = new List<Expression<Func<TEntity, object>>>();
        public Expression<Func<TEntity, object>> OrderBy { get; set; } = null;
        public Expression<Func<TEntity, object>> OrderByDesc { get; set; } = null;
        public int Take { get; set; } 
        public int Skip { get; set; } 
        public bool IsPagination { get; set; }

        public BaseSpecifications(Expression<Func<TEntity, bool>> expression)
        {
            Criteria = expression;
        }

        public BaseSpecifications()
        {
            
        }

        public void AddOrderBy(Expression<Func<TEntity, object>> expression)
        {
            OrderBy = expression;
        }
        public void AddOrderByDesc(Expression<Func<TEntity, object>> expression)
        {
            OrderByDesc = expression;
        }

        public void ApplyPagination(int take , int skip)
        {
            IsPagination = true;
            Take = take;
            Skip = skip;
        }

    }
}
