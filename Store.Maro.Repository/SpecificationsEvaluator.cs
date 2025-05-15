using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Store.Maro.Core.Entities;
using Store.Maro.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Maro.Repository
{
    public static class SpecificationEvaluator<TEntity,TKey> where TEntity : BaseEntity<TKey>
    {
        // generte the query 
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery , ISpecifications<TEntity,TKey> spec)
        {
            // inputQuery is Dbcontext + entity e.g _context.Product
            // then add the specifications to it include, order , where , search

            var query = inputQuery; //_context.entity

            // add a criteria to the query
            if(spec.Criteria is not null)
            {
                query = query.Where(spec.Criteria);
            }

            if(spec.OrderBy is not null)
            {
                query = query.OrderBy(spec.OrderBy);
            }

            if (spec.OrderByDesc is not null)
            {
                query = query.OrderByDescending(spec.OrderByDesc);
            }

            if(spec.IsPagination)
            {
                query = query.Skip(spec.Skip).Take(spec.Take);
            }

            // add the includes to query if exist

            #region Aggregation Explanition
            //query.Include(spec.Includes[0]);
            //query.Include(spec.Includes[1]);

            // aggregate take intial value which is query (seed) then assigned it to currQuery and then iterate
            // For each includeExpression in spec.Includes, do:
            //      currQuery = currQuery.Include(includeExpression);
            //Finally, assign the fully constructed query back to query.

            // e.g _context.product 
            // _context.product.include(P=>P.Brand)
            // _context.product.include(P=>P.Brand).include(P=>P.type)
            // and so on if there exist other includes 
            #endregion


            query = spec.Includes.Aggregate(query, (currQuery, includeExpression) => currQuery.Include(includeExpression));


            return query;
        }
    }
}
