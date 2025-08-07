using ExcelReader.RyanW84.Abstractions.Data.TableCreators;
using ExcelReader.RyanW84.Abstractions.FileOperations.Readers;
using ExcelReader.RyanW84.Data;

namespace ExcelReader.RyanW84.Controller;

public class AnyExcelReadController(
	IExcelReaderDbContext dbContext ,
	IAnyExcelReader anyExcelReader ,
	IExcelTableCreator createTableFromExcel)
{
	private readonly IExcelReaderDbContext _dbContext = dbContext;
	private readonly IAnyExcelReader _anyExcelReader = anyExcelReader;
	private readonly IExcelTableCreator _createTableFromExcel = createTableFromExcel;

	public async Task AddDynamicDataFromExcel()
	{
		var dataTable = await _anyExcelReader.ReadFromExcelAsync();
		_createTableFromExcel.CreateTableFromExcel(dataTable);
		await _dbContext.SaveChangesAsync();
	}
}