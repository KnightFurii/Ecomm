using System;
using Core.Entities;

namespace Core.Interfaces;

public interface IGenericrepository<T> where T : BaseEntity 
{
    Task<T?> GetbByIdAsync(int id) ;
    Task<IReadOnlyList<T>> ListAllAsync();
    Task<T?> GetEntityWithSpec(ISpecifications<T> spec);
    Task<IReadOnlyList<T>> ListAsync(ISpecifications<T> spec);
    Task<TResult?> GetEntityWithSpec<TResult>(ISpecifications<T,TResult> spec);
    Task<IReadOnlyList<TResult>> ListAsync<TResult>(ISpecifications<T, TResult> spec);
    void Add(T entity);
    void Update(T entity);
    void Remove(T entity);
    Task<bool> SaveAllAsync();
    bool Exists(int id);
    Task<int> CountAsync(ISpecifications<T> spec);
}