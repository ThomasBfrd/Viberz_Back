namespace Viberz.Domain.Interfaces;

public interface IBaseRepository<T> where T : class
{
    Task Add(T entity);
}
