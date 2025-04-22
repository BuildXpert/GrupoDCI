using Microsoft.EntityFrameworkCore;
using Build_Xpert.Model;

namespace Build_Xpert.Repository.Base;
/// <summary>
/// Base class for repository operations.
/// </summary>
/// <typeparam name="T">Entity type.</typeparam>
public class RepositoryBase<T, TKey> : IRepositoryBase<T, TKey> where T : class
{
    private readonly ApplicationDbContext _context;
    protected ApplicationDbContext DbContext => _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="RepositoryBase{T}"/> class.
    /// </summary>
    public RepositoryBase(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Creates an entity of type T asynchronously.
    /// </summary>
    /// <param name="entity">The entity to be saved in the database.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating success.</returns>
    public async Task<bool> CreateAsync(T entity)
    {
        try
        {
            await _context.AddAsync(entity);
            return await SaveAsync();
        }
        catch
        {
            throw;
        }
    }
    //public async Task<int> CreateReturnIdAsync(T entity)    {
    //    try
    //    {
    //        await _context.AddAsync(entity);
    //        return await SaveAsync();
    //    }
    //    catch
    //    {
    //        throw;
    //    }
    //}

    /// <summary>
    /// Updates an existing entity of type T asynchronously.
    /// </summary>
    /// <param name="entity">The entity to be updated.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating success.</returns>
    public async Task<bool> UpdateAsync(T entity)
    {
        try
        {
            _context.Update(entity);
            return await SaveAsync();
        }
        catch
        {
            throw;
        }
    }

    /// <summary>
    /// Updates multiple entities of type T asynchronously.
    /// </summary>
    /// <param name="entities">The collection of entities to be updated.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating success.</returns>
    public async Task<bool> UpdateManyAsync(IEnumerable<T> entities)
    {
        try
        {
            _context.UpdateRange(entities);
            return await SaveAsync();
        }
        catch
        {
            throw;
        }
    }

    /// <summary>
    /// Deletes an entity of type T asynchronously.
    /// </summary>
    /// <param name="entity">The entity to be deleted.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating success.</returns>
    public async Task<bool> DeleteAsync(T entity)
    {
        try
        {
            _context.Remove(entity);
            return await SaveAsync();
        }
        catch
        {
            throw;
        }
    }

    /// <summary>
    /// Reads all entities of type T asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of entities.</returns>
    public async Task<IEnumerable<T>> ReadAsync()
    {
        try
        {
            return await _context.Set<T>().ToListAsync();
        }
        catch
        {
            throw;
        }
    }

    public IQueryable<T> ReadQueriableAsync()
    {
        try
        {
            return _context.Set<T>().AsQueryable();
        }
        catch
        {
            throw;
        }
    }

    /// <summary>
    /// Checks if an entity of type T exists asynchronously.
    /// </summary>
    /// <param name="entity">The entity to check for existence.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating if the entity exists.</returns>
    public async Task<bool> BExistsAsync(T entity)
    {
        try
        {
            var items = await ReadAsync();
            return items.Any(x => x.Equals(entity));
        }
        catch
        {
            throw;
        }
    }

    /// <summary>
    /// Saves changes to the database asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating success.</returns>
    protected async Task<bool> SaveAsync()
    {
        var result = await _context.SaveChangesAsync();
        return result > 0;
    }

    public async Task<T> ReadOneAsync(TKey id)
    {
        try
        {
            return await _context.Set<T>().FindAsync(id);
        }
        catch
        {
            throw;
        }
    }
}

public class PAWException : Exception
{
    public PAWException() { }

    public PAWException(string message) : base(message) { }

    public PAWException(string message, Exception innerException) : base(message, innerException) { }

    public PAWException(Exception innerException) : base("An error occurred.", innerException) { }
}
