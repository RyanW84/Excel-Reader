using ExcelReader.RyanW84.Controller;
using ExcelReader.RyanW84.Data;
using ExcelReader.RyanW84.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ExcelReader.RyanW84
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            ProcessExcelData(host.Services);
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    // Access the connection string
                    var connectionString = hostContext.Configuration.GetConnectionString("DefaultConnection");
                    Console.WriteLine($"Connection String: {connectionString}"); // Debugging line

                    // Register services
                    services.AddDbContext<ExcelReaderDbContext>(options =>
                        options.UseSqlServer(connectionString));
                    services.AddScoped<CreateTableFromAnyExcel>();
                    services.AddScoped<ExcelBeginnerController>();
                    services.AddScoped<AnyExcelRead>();
                });
        }

        private static void ProcessExcelData(IServiceProvider services)
        {
            // Create a new scope for the first operation
            using (var scope = services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ExcelReaderDbContext>();
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                var excelBeginnerController = scope.ServiceProvider.GetRequiredService<ExcelBeginnerController>();
                excelBeginnerController.AddDataFromExcel();
            }

            // Create a new scope for the second operation
            using (var scope = services.CreateScope())
            {
                var anyExcelRead = scope.ServiceProvider.GetRequiredService<AnyExcelRead>();
                var dataTable = anyExcelRead.ReadFromExcel();

                var createTableFromAnyExcel = scope.ServiceProvider.GetRequiredService<CreateTableFromAnyExcel>();
                createTableFromAnyExcel.CreateTableFromExcel(dataTable);

                // Save changes to the new context
                var context = scope.ServiceProvider.GetRequiredService<ExcelReaderDbContext>();
                context.SaveChanges();
            }
        }
    }
}