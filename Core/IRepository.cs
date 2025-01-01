namespace Core;

public interface IRepository<T>
{
    IUnitOfWork UnitOfWork { get; }
    Task<IEnumerable<T>> GetAll();
    Task<T> GetById(Guid id);
    Task<T> Insert(T obj);
    void Update(T obj);
    Task Delete(Guid id);
    Task<bool> Exists(Guid id);
}