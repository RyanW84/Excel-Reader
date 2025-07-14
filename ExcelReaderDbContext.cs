using ExcelReader.RyanW84.Models;
using Microsoft.EntityFrameworkCore;

namespace ExcelReader.RyanW84;

public class ExcelReaderDbContext
{
    
    public DbSet<ExcelBeginner>? ExcelBeginners { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .Entity<ExcelBeginner>()
            .HasOne();
    }

    public void SeedData()
    {
        ExcelBeginners.RemoveRange(ExcelBeginners);

        var excelBeginners = new List<ExcelBeginner>
        {
            new ExcelBeginner
            {
                Name = "Test",
                Columns = "1",
                Rows = "1",
                Values = "1"
            },

            new ExcelBeginner
            {
                Name = "Test2",
                Columns = "2",
                Rows = "2",
                Values = "2"
            },
            new ExcelBeginner
            {
                Name = "Test3",
                Columns = "3",
                Rows = "3",
                Values = "3"
            },
        };
                ExcelBeginners.AddRange(excelBeginners);
                SaveChanges();
                
                
    }
}