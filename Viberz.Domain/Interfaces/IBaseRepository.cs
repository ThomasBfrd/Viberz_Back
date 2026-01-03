namespace Viberz.Domain.Interfaces;

public interface IBaseRepository<T, TId> where T : class
{
    Task Add(T entity);
    Task Delete(T entity);
    Task<T?> GetByIdAsync(TId id);
}
