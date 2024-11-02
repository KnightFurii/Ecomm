using System;
using System.Linq.Expressions;

namespace Core.Interfaces;

public interface ISpecifications<T>
{
    Expression<Func<T, bool>>? Criteria{get;}
    Expression<Func<T, object>>? OrderBy {get;}
    Expression<Func<T, Object>>? OrderbyDesc {get;}
    bool isDistinct {get;}
}

public interface ISpecifications<T, TResult> : ISpecifications<T>
{
    Expression<Func<T, TResult>>? Select {get;}
}