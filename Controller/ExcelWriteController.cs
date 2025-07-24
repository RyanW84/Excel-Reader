using System.Data;
using ExcelReader.RyanW84.Services;

namespace ExcelReader.RyanW84.Controller;

public class ExcelWriteController(WriteToExcelService writeToExcelService , AnyExcelRead anyExcelRead)
{
    private readonly WriteToExcelService _writeToExcelService = writeToExcelService;
    private readonly AnyExcelRead _anyExcelRead = anyExcelRead;

	public void WriteDataToExcel(string filePath, Dictionary<string, string> fieldValues)
    {
        _writeToExcelService.WriteFieldsToExcel(filePath, fieldValues);
    }

    public Dictionary<string, string> GetExistingFieldValues(string filePath)
    {
        var table = _anyExcelRead.ReadFromExcel();
        var result = new Dictionary<string, string>();
        if (table.Rows.Count > 0)
        {
            foreach (DataColumn col in table.Columns)
            {
                result[col.ColumnName] = table.Rows[0][col].ToString() ?? string.Empty;
            }
        }
        return result;
    }
}
