namespace Core;

public interface IRepository<T>
{
    Task<IEnumerable<T>> Get();
    Task<T> Get(Guid id);
    Task<T> Insert(T obj);
    void Update(T obj);
    Task Delete(Guid id);
    Task<bool> Exists(Guid id);
}