using System.Data;
using System.Data.SqlClient;

namespace ExcelReader.RyanW84.Abstractions;

/// <summary>
/// Interface for table existence and management operations
/// </summary>
public interface ITableManager
{
    void EnsureTableExists(DataTable dataTable, SqlConnection connection);
    void EnsureTableExists(DataTable dataTable);
}