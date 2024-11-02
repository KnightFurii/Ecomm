using System;
using Core.Entities;
using Core.Interfaces;

namespace Infra.Data;

public class SpecificationEvaluator<T> where T : BaseEntity
{
    public static IQueryable<T> GetQuery(IQueryable<T> query,ISpecifications<T> spec)
    {
        if (spec.Criteria != null)
        {
            query = query.Where(spec.Criteria);
        }

        if(spec.OrderBy != null)
        {
            query = query.OrderBy(spec.OrderBy);
        }

        if(spec.OrderbyDesc != null)
        {
            query = query.OrderByDescending(spec.OrderbyDesc);
        }

        if(spec.isDistinct)
        {
            query = query.Distinct();   
        }

        return query;
    }

    public static IQueryable<TResult> GetQuery<TSpec, TResult>(IQueryable<T> query,
    ISpecifications<T, TResult> spec)
    {
        if (spec.Criteria != null)
        {
            query = query.Where(spec.Criteria);
        }

        if(spec.OrderBy != null)
        {
            query = query.OrderBy(spec.OrderBy);
        }

        if(spec.OrderbyDesc != null)
        {
            query = query.OrderByDescending(spec.OrderbyDesc);
        }

        var selectQuery = query as IQueryable<TResult>;

        if(spec.Select!=null)
        {
            selectQuery = query.Select(spec.Select);
        }

        if(spec.isDistinct)
        {
            selectQuery = selectQuery?.Distinct();
        }
        return selectQuery ?? query.Cast<TResult>();
    }
}
