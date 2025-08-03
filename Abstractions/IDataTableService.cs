using System.Data;

namespace ExcelReader.RyanW84.Abstractions;

/// <summary>
/// Interface for DataTable operations and conversions
/// </summary>
public interface IDataTableService
{
    DataTable Convert(Dictionary<string, object> dictionary);
}