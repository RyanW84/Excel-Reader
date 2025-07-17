using ExcelReader.RyanW84.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Logging;

namespace ExcelReader.RyanW84.Data;

public class ExcelReaderDbContext() : DbContext
{
    private DbContextOptions<ExcelReaderDbContext> ConnectionString { get; } 
    public DbSet<ExcelBeginner> ExcelBeginner { get; set; }


    private static ILoggerFactory GetLoggerFactory()
    {
        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
            builder.AddFilter((category, level) =>
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
                new List<ExcelBeginner>
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
                    }
                }
            );
    }

    public class ExcelDbContextFactory : IDesignTimeDbContextFactory<ExcelReaderDbContext>
    {
        public ExcelReaderDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ExcelReaderDbContext>();
            optionsBuilder.UseSqlServer(
                "Server=(localdb)\\MSSQLlocaldb; Database = ExcelReader; initial Catalog =ExcelReader; integrated security = True;MultipleActiveResultSets=True; ");
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.UseLoggerFactory(GetLoggerFactory());
            // TODO: optionsBuilder.Add

            return new ExcelReaderDbContext();
        }
    }
}