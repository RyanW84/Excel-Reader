using ExcelReader.RyanW84.Abstractions;
using ExcelReader.RyanW84.Data;
using ExcelReader.RyanW84.Models;
using Microsoft.EntityFrameworkCore;

namespace ExcelReader.RyanW84.Repositories;

/// <summary>
/// ExcelBeginner repository implementation following Repository pattern
/// </summary>
public class ExcelBeginnerRepository(ExcelReaderDbContext context) : BaseRepository<ExcelBeginner>(context), IExcelBeginnerRepository
{
	public async Task<IEnumerable<ExcelBeginner>> GetByNameAsync(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return [];

        return await DbSet
            .Where(x => x.Name.Contains(name))
            .ToListAsync();
    }

    public async Task<IEnumerable<ExcelBeginner>> GetByAgeRangeAsync(int minAge, int maxAge)
    {
        return await DbSet
            .Where(x => x.Age >= minAge && x.Age <= maxAge)
            .ToListAsync();
    }
}