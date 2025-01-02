using Core;
using Core.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;


public abstract class Repository<T> : IRepository<T> where T : class
{
    private readonly DbSet<T> _table;
    private readonly ApplicationDbContext _context;
    protected Repository(ApplicationDbContext context)
    {
        _context = context;
        _table = context.Set<T>();
    }

    public virtual async Task<IEnumerable<T>> Get()
    {
        return await _table.ToListAsync();
    }

    public virtual async Task<T> Get(Guid id)
    {
        var result =  await _table.FindAsync(id);
        if (result == null)
        {
            throw new NotFoundException(id.ToString());
        }

        return result;
    }
    
    public virtual async Task<T> Insert(T obj)
    {
        var e = await _table.AddAsync(obj);
        return e.Entity;
    }

    //This method is going to update the record in the table
    //It will receive the object as an argument
    public virtual void Update(T obj)
    {
        //First attach the object to the table
        _table.Attach(obj);
        //Then set the state of the Entity as Modified
        _context.Entry(obj).State = EntityState.Modified;
    }
    
    public virtual async Task Delete(Guid id)
    {
        //First, fetch the record from the table

        var existing = await Get(id);
        _table.Remove(existing);
    }

    public virtual async Task<bool> Exists(Guid id)
    {
        try
        {
            await Get(id);
            return true;
        }
        catch(NotFoundException)
        {
            return false;
        }
    }
}