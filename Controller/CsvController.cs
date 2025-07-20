using ExcelReader.RyanW84.Data;
using ExcelReader.RyanW84.Services;
using Microsoft.Extensions.Configuration;

namespace ExcelReader.RyanW84.Controller;

public class CsvController(IConfiguration configuration, ExcelReaderDbContext dbContext, ReadFromCsv readFromCsv, CreateTableFromCSV createTableFromCSV)
{
    public void AddDataFromCsv()
    {
        Console.WriteLine("\nStarting CSV import...");
        var csvData = readFromCsv.ReadCsvFile();
        var dataTable = readFromCsv.ConvertToDataTable(csvData);
        Console.WriteLine($"Read {dataTable.Rows.Count} Rows from CSV file.");
		Console.WriteLine($"Read {dataTable.Columns.Count} Columns from CSV file.");

		dataTable.TableName = "CsvImport";
        createTableFromCSV.CreateTableFromCsvData(dataTable);
        dbContext.SaveChanges();
        Console.WriteLine("CSV import complete.");
    }
}