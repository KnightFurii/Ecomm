using System;
using System.Linq.Expressions;
using Core.Interfaces;

namespace Core.Specifications;

public class BaseSpecification<T> (Expression<Func<T, bool>>? criteria): ISpecifications<T>
{
    protected BaseSpecification() : this(null!){}
    public Expression<Func<T, bool>>? Criteria => criteria;
    public Expression<Func<T, object>>? OrderBy {get; private set;}
    public Expression<Func<T, Object>>? OrderbyDesc {get; private set;}

    public bool isDistinct {get; private set;}

    protected void AddorderBy(Expression<Func<T, object>>? orderByExpression)
    {
        OrderBy = orderByExpression;
    }

    protected void AddorderByDesc(Expression<Func<T, object>>? orderByDescExpression)
    {
        OrderbyDesc = orderByDescExpression;
    }

    protected void ApplyDistinct(){
        isDistinct =true;
    }
}

public class BaseSpecification<T, TResult>(Expression<Func<T, bool>> criteria) : BaseSpecification<T>(criteria), ISpecifications<T, TResult>
{
    protected BaseSpecification() : this(null!){}

    public Expression<Func<T, TResult>>? Select {get; private set;}

    protected void Addselect(Expression<Func<T, TResult>> selectExpression)
    {
        Select = selectExpression;
    }
}
