// Ignore Spelling: dict

using System.Data;

using ExcelReader.RyanW84.Data;

namespace ExcelReader.RyanW84.Services;

public class WriteUpdatedExcelDataToDatabase(
	DictionaryToDataTableConverter dictToTableConverter ,
	CreateTableFromAnyExcel createTableFromAnyExcel ,
	ExcelReaderDbContext dbContext)
{
	private readonly DictionaryToDataTableConverter _dictToTableConverter = dictToTableConverter;
	private readonly CreateTableFromAnyExcel _createTableFromAnyExcel = createTableFromAnyExcel;
	private readonly ExcelReaderDbContext _dbContext = dbContext;

	public async Task WriteAsync(Dictionary<string , string> fieldValues)
		{
		// Convert string dictionary to object dictionary for the converter
		var objDict = new Dictionary<string , object>(fieldValues.Count);
		foreach(var kvp in fieldValues)
			{
			objDict[kvp.Key] = kvp.Value;
			}

		// Convert to DataTable
		var dataTable = _dictToTableConverter.Convert(objDict);
		dataTable.TableName = "ExcelData";

		// Use the existing service to create and populate the table
		_createTableFromAnyExcel.CreateTableFromExcel(dataTable);
		await _dbContext.SaveChangesAsync();
		}

	// Keep synchronous version for backward compatibility during transition
	public void Write(Dictionary<string , string> fieldValues)
		{
		WriteAsync(fieldValues).GetAwaiter().GetResult();
		}
	}