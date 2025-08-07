using System.Data;

namespace ExcelReader.RyanW84.Abstractions.FileOperations.Readers;

/// <summary>
/// Interface for any Excel reading operations
/// </summary>
public interface IAnyExcelReader
{
    Task<DataTable> ReadFromExcelAsync();
    DataTable ReadFromExcel();
}