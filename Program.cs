using ExcelReader.RyanW84.Controller;
using ExcelReader.RyanW84.Data;
using ExcelReader.RyanW84.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ExcelReader.RyanW84;

public class Program(IConfiguration configuration)
{
    public static void Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();

        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;

        var program = services.GetRequiredService<Program>();
        program.ProcessExcelData();
    }

    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddScoped<Program>();
                services.AddScoped<ExcelReaderDbContext>();
                services.AddScoped<CreateTableFromAnyExcel>();
                services.AddScoped<ExcelBeginnerController>();
                services.AddScoped<AnyExcelRead>();
                
                
            });
    }

    private void ProcessExcelData()
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        var context = new ExcelReaderDbContext();
        if (connectionString is not null)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }

        // Get the controller from the service provider instead of creating it manually
        var host = CreateHostBuilder([]).Build();
        using var scope = host.Services.CreateScope();
        var excelBeginnerController = scope.ServiceProvider.GetRequiredService<ExcelBeginnerController>();
        excelBeginnerController.AddDataFromExcel();
    }
}