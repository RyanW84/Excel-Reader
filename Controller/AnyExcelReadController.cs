using ExcelReader.RyanW84.Data;
using ExcelReader.RyanW84.Services;
using Microsoft.Extensions.Configuration;

namespace ExcelReader.RyanW84.Controller;

public class AnyExcelReadController(IConfiguration configuration, ExcelReaderDbContext dbContext)
{
    private readonly IConfiguration _configuration = configuration;

    // Inject both IConfiguration and ExcelReaderDbContext

    public void AddDataFromExcel()
    {
        // Use the injected dbContext directly
        Console.WriteLine("opening DB Context");
        using var db = dbContext;
        
        Console.WriteLine("Reading from Dynamic Excel Spreadsheet");
        var anyExcelRead = new AnyExcelRead(_configuration);
        
        Console.WriteLine("Creating Table");
        var dataTable = anyExcelRead.ReadFromExcel();
        var createTableFromAnyExcel = new CreateTableFromAnyExcel(_configuration);
        createTableFromAnyExcel.CreateTableFromExcel(dataTable);
        db.SaveChanges();
    }
}