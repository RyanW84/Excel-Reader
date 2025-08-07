using System.Data;
using System.Data.SqlClient;

using ExcelReader.RyanW84.Abstractions.Core;

using Microsoft.Extensions.Configuration;

namespace ExcelReader.RyanW84.Helpers;

public class TableExistence : ITableManager
{
    private readonly IConfiguration _configuration;

    public TableExistence(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void EnsureTableExists(DataTable dataTable, SqlConnection connection)
    {
        var columnDefs = new List<string>();
        foreach (DataColumn column in dataTable.Columns)
        {
            var columnType = GetSqlDataType(column.DataType);
            columnDefs.Add($"[{column.ColumnName}] {columnType}");
        }

        var checkTableSql =
            $"IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='{dataTable.TableName}' AND xtype='U') "
            + $"CREATE TABLE [{dataTable.TableName}] ({string.Join(", ", columnDefs)})";

        using var command = new SqlCommand(checkTableSql, connection);
        command.ExecuteNonQuery();
    }

    public void EnsureTableExists(DataTable dataTable)
    {
        var connectionString = _configuration.GetConnectionString("DefaultConnection");
        using var connection = new SqlConnection(connectionString);
        connection.Open();
        EnsureTableExists(dataTable, connection);
    }

    private static string GetSqlDataType(Type dataType)
    {
        return dataType switch
        {
            Type t when t == typeof(string) => "NVARCHAR(MAX)",
            Type t when t == typeof(int) => "INT",
            Type t when t == typeof(DateTime) => "DATETIME",
            Type t when t == typeof(decimal) => "DECIMAL(18,2)",
            Type t when t == typeof(double) => "FLOAT",
            Type t when t == typeof(bool) => "BIT",
            _ => "NVARCHAR(MAX)"
        };
    }
}