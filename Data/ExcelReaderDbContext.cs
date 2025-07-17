using ExcelReader.RyanW84.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace ExcelReader.RyanW84.Data
{
    public class ExcelReaderDbContext(DbContextOptions<ExcelReaderDbContext> options) : DbContext(options)
    {
        public DbSet<ExcelBeginner> ExcelBeginner { get; set; }

        public static ILoggerFactory GetLoggerFactory()
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
                        new() { Id = 1, Name = "Name", age = 1, sex = "NA", colour = "NA", height = "F000" },
                        new() { Id = 2, Name = "Name2", age = 2, sex = "NA", colour = "NA", height = "F200" },
                        new() { Id = 3, Name = "Name3", age = 3, sex = "NA", colour = "NA", height = "F300" }
                    }
                );
        }
    }

    public class ExcelDbContextFactory : IDesignTimeDbContextFactory<ExcelReaderDbContext>
    {
        public ExcelReaderDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ExcelReaderDbContext>();
            optionsBuilder.UseSqlServer(
                "Server=(localdb)\\MSSQLlocaldb; Database=ExcelReader; Integrated Security=True; MultipleActiveResultSets=True;")
                .EnableSensitiveDataLogging()
                .UseLoggerFactory(ExcelReaderDbContext.GetLoggerFactory());

            return new ExcelReaderDbContext(optionsBuilder.Options);
        }
    }

    // Extension method for adding DbContext to the service collection
    public class ServiceCollectionExtensions
    {
        public  IServiceCollection AddExcelReaderDbContext(IServiceCollection services, string connectionString)
        {
            services.AddDbContext<ExcelReaderDbContext>(optionsAction: options =>
                options.UseSqlServer(@"Server=(localdb)\\MSSQLlocaldb; Database=ExcelReader; Integrated Security=True; MultipleActiveResultSets=True;")
                       .UseLoggerFactory(loggerFactory: ExcelReaderDbContext.GetLoggerFactory())
                       .EnableSensitiveDataLogging());

            return services;
        }
    }
}
