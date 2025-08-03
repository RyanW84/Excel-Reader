using System.Data;
using ExcelReader.RyanW84.Abstractions;
using ExcelReader.RyanW84.Data;
using Microsoft.EntityFrameworkCore;

namespace ExcelReader.RyanW84.Repositories;

/// <summary>
/// Base repository implementation following Repository pattern and DRY principle
/// </summary>
/// <typeparam name="T">Entity type</typeparam>
public abstract class BaseRepository<T>(ExcelReaderDbContext context) : IRepository<T> where T : class
{
    protected readonly ExcelReaderDbContext Context = context ?? throw new ArgumentNullException(nameof(context));
    protected readonly DbSet<T> DbSet = context.Set<T>();

	public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await DbSet.ToListAsync();
    }

    public virtual async Task<T?> GetByIdAsync(int id)
    {
        return await DbSet.FindAsync(id);
    }

    public virtual async Task<T> AddAsync(T entity)
    {
		ArgumentNullException.ThrowIfNull(entity);

		var result = await DbSet.AddAsync(entity);
        return result.Entity;
    }

    public virtual async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities)
    {
		ArgumentNullException.ThrowIfNull(entities);

		var entitiesList = entities.ToList();
        await DbSet.AddRangeAsync(entitiesList);
        return entitiesList;
    }

    public virtual async Task<T> UpdateAsync(T entity)
    {
		ArgumentNullException.ThrowIfNull(entity);

        DbSet.Update(entity);
        return await Task.FromResult(entity);
    }

    public virtual async Task DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
        {
            DbSet.Remove(entity);
        }
    }

    public virtual async Task<bool> ExistsAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        return entity != null;
    }

    public virtual async Task<int> SaveChangesAsync()
    {
        return await Context.SaveChangesAsync();
    }
}