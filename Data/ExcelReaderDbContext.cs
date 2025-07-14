using ExcelReader.RyanW84.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ExcelReader.RyanW84.Data;

internal class ExcelReaderDbContext : DbContext
{
    public DbSet<ExcelBeginner> ExcelBeginner { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder
            .UseSqlServer(
                @"Server=(localdb)\MSSQLlocaldb; Database = ExcelReaderDB; initial Catalog=ExcelReaderDB; Integrated Security=True; TrustServerCertificate=True;"
            )
            .EnableSensitiveDataLogging()
            .UseLoggerFactory(GetLoggerFactory());
    }

    private static ILoggerFactory GetLoggerFactory()
    {
        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
            builder.AddFilter(
                (category, level) =>
                    category == DbLoggerCategory.Database.Command.Name
                    && level == LogLevel.Information
            );
        });
        return loggerFactory;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<ExcelBeginner>()
            .HasData(
                new List<ExcelBeginner>()
                {
                    new()
                    {
                        Id = 1,
                        Name = "Name",
                        age = 1,
                        sex = "NA",
                        colour = "NA",
                        height = "F000"
                    },
                    new()
                    {
                        Id = 2,
                        Name = "Name2",
                        age = 2,
                        sex = "NA",
                        colour = "NA",
                        height = "F200"
                    },
                    new()
                    {
                        Id = 3,
                        Name = "Name3",
                        age = 3,
                        sex = "NA",
                        colour = "NA",
                        height = "F300"
                    },
                }
            );
    }
}
