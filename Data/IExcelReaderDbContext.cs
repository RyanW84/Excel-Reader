using ExcelReader.RyanW84.Models;
using Microsoft.EntityFrameworkCore;

namespace ExcelReader.RyanW84.Data
{
    public interface IExcelReaderDbContext : IDisposable
    {
        DbSet<ExcelBeginner> ExcelBeginner { get; set; }
        
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        int SaveChanges();
        
        void EnsureDeleted();
        void EnsureCreated();
    }
}