using ExcelReader.RyanW84.Models;
using ExcelReader.RyanW84.Abstractions.Base;

namespace ExcelReader.RyanW84.Abstractions.Data.Repositories;

/// <summary>
/// Specific repository interface for ExcelBeginner entities
/// </summary>
public interface IExcelBeginnerRepository : IRepository<ExcelBeginner>
{
    Task<IEnumerable<ExcelBeginner>> GetByNameAsync(string name);
    Task<IEnumerable<ExcelBeginner>> GetByAgeRangeAsync(int minAge, int maxAge);
}