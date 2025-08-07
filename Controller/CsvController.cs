using System.Data;
using ExcelReader.RyanW84.Abstractions.Core;
using ExcelReader.RyanW84.Abstractions.Data.DatabaseServices;
using ExcelReader.RyanW84.Abstractions.Data.TableCreators;
using ExcelReader.RyanW84.Abstractions.FileOperations.Readers;
using ExcelReader.RyanW84.Abstractions.Services;
using ExcelReader.RyanW84.Data;

namespace ExcelReader.RyanW84.Controller;

public class CsvController(
	IExcelReaderDbContext dbContext ,
	ICsvFileReader csvFileReader ,
	ICsvTableCreator createTableFromCSV ,
	INotificationService notificationService ,
	IDataConverter<List<string[]> , DataTable> csvDataConverter
	)
{
    private readonly IExcelReaderDbContext _dbContext = dbContext;
    private readonly ICsvFileReader _csvFileReader = csvFileReader;
    private readonly ICsvTableCreator _createTableFromCSV = createTableFromCSV;
    private readonly INotificationService _notificationService = notificationService;
    private readonly IDataConverter<List<string[]>, DataTable> _csvDataConverter = csvDataConverter;

	public async Task AddDataFromCsv()
    {
        _notificationService.ShowInfo("Starting CSV import...");
        var csvData = await _csvFileReader.ReadCsvFile();
        var dataTable = await _csvDataConverter.ConvertAsync(csvData);
        _notificationService.ShowInfo($"Read {dataTable.Rows.Count} Rows from CSV file.");
        _notificationService.ShowInfo($"Read {dataTable.Columns.Count} Columns from CSV file.");

        dataTable.TableName = "CsvImport";
        await _createTableFromCSV.CreateTableFromCsvDataAsync(dataTable);
        await _dbContext.SaveChangesAsync();
        _notificationService.ShowSuccess("CSV import complete.");
    }
}
// Note: The above code assumes that the ICsvFileReader and ICsvTableCreator interfaces
