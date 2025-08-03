using ExcelReader.RyanW84.Data;
using ExcelReader.RyanW84.Services;
using ExcelReader.RyanW84.Helpers;

namespace ExcelReader.RyanW84.Controller;

public class CsvController
{
    private readonly ExcelReaderDbContext _dbContext;
    private readonly ReadFromCsv _readFromCsv;
    private readonly CreateTableFromCSV _createTableFromCSV;
    private readonly UserNotifier _userNotifier;

    public CsvController(ExcelReaderDbContext dbContext, ReadFromCsv readFromCsv, CreateTableFromCSV createTableFromCSV, UserNotifier userNotifier)
    {
        _dbContext = dbContext;
        _readFromCsv = readFromCsv;
        _createTableFromCSV = createTableFromCSV;
        _userNotifier = userNotifier;
    }

    public async Task AddDataFromCsv()
    {
        _userNotifier.ShowInfo("Starting CSV import...");
        var csvData = await _readFromCsv.ReadCsvFile();
        var dataTable = await _readFromCsv.ConvertToDataTableAsync(csvData);
        _userNotifier.ShowInfo($"Read {dataTable.Rows.Count} Rows from CSV file.");
        _userNotifier.ShowInfo($"Read {dataTable.Columns.Count} Columns from CSV file.");

        dataTable.TableName = "CsvImport";
        await _createTableFromCSV.CreateTableFromCsvDataAsync(dataTable);
        await _dbContext.SaveChangesAsync();
        _userNotifier.ShowSuccess("CSV import complete.");
    }
}