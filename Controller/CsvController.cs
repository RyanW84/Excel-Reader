using ExcelReader.RyanW84.Data;
using ExcelReader.RyanW84.Services;
using Microsoft.Extensions.Configuration;

namespace ExcelReader.RyanW84.Controller;

public class CsvController(IConfiguration configuration, ExcelReaderDbContext dbContext, ReadFromCsv readFromCsv, CreateTableFromCSV createTableFromCSV)
{
    private readonly IConfiguration _configuration = configuration;
    private readonly ExcelReaderDbContext _dbContext = dbContext;
    private readonly ReadFromCsv _readFromCsv = readFromCsv;
    private readonly CreateTableFromCSV _createTableFromCSV = createTableFromCSV;

    public void AddDataFromCsv()
    {
        Console.WriteLine("\nStarting CSV import...");
        var csvData = _readFromCsv.ReadCsvFile();
        var dataTable = _readFromCsv.ConvertToDataTable(csvData);
        Console.WriteLine($"Read {dataTable.Rows.Count} Rows from CSV file.");
        Console.WriteLine($"Read {dataTable.Columns.Count} Columns from CSV file.");

        dataTable.TableName = "CsvImport";
        _createTableFromCSV.CreateTableFromCsvData(dataTable);
        _dbContext.SaveChanges();
        Console.WriteLine("CSV import complete.");
    }
}