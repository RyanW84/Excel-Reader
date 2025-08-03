namespace ExcelReader.RyanW84.Abstractions;

/// <summary>
/// Interface for Excel writing operations
/// </summary>
public interface IExcelWriteService
{
    void WriteFieldsToExcel(string filePath, Dictionary<string, string> fieldValues);
}