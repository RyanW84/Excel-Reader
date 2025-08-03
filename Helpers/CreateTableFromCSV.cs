﻿using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

using System.Data;
using System.Text;

namespace ExcelReader.RyanW84.Helpers;

public class CreateTableFromCSV
{
    private readonly IConfiguration _configuration;

    public CreateTableFromCSV(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task CreateTableFromCsvDataAsync(DataTable dataTable)
    {
        await Task.Run(() => CreateTableFromCsvData(dataTable));
    }

    // Keep synchronous version for backward compatibility
    public void CreateTableFromCsvData(DataTable dataTable)
    {
        if (dataTable == null)
            throw new ArgumentNullException(nameof(dataTable));

        if (string.IsNullOrWhiteSpace(dataTable.TableName))
            throw new ArgumentException("Table name cannot be empty", nameof(dataTable));

        if (dataTable.Columns.Count == 0)
            throw new ArgumentException("DataTable must have at least one column", nameof(dataTable));

        // Validate column names and sanitize if needed
        for (int i = 0; i < dataTable.Columns.Count; i++)
        {
            var column = dataTable.Columns[i];
            if (string.IsNullOrWhiteSpace(column.ColumnName))
            {
                column.ColumnName = $"Column_{i + 1}";
            }
            // Replace invalid characters and spaces
            column.ColumnName = SanitizeColumnName(column.ColumnName);
        }

        var tableName = SanitizeTableName(dataTable.TableName);
        var connectionString = _configuration.GetConnectionString("DefaultConnection");

        using var connection = new SqlConnection(connectionString);
        connection.Open();

        // Build CREATE TABLE statement
        var createTableSql = BuildCreateTableStatement(tableName, dataTable.Columns);

        // Create the table
        using (var command = new SqlCommand(createTableSql, connection))
        {
            command.ExecuteNonQuery();
        }

        // Bulk copy the data
        using var bulkCopy = new SqlBulkCopy(connection);
        bulkCopy.DestinationTableName = tableName;
        bulkCopy.WriteToServer(dataTable);
    }

    private string BuildCreateTableStatement(string tableName, DataColumnCollection columns)
    {
        var sb = new StringBuilder($"CREATE TABLE [{tableName}] (\n");

        for (int i = 0; i < columns.Count; i++)
        {
            var column = columns[i];
            var sqlDataType = GetSqlDataType(column.DataType);
            sb.Append($"[{column.ColumnName}] {sqlDataType}");

            if (i < columns.Count - 1)
                sb.Append(",\n");
        }

        sb.Append("\n)");
        return sb.ToString();
    }

    private string GetSqlDataType(Type type)
    {
        return type.Name.ToLower() switch
        {
            "string" => "NVARCHAR(MAX)",
            "int32" => "INT",
            "int64" => "BIGINT",
            "decimal" => "DECIMAL(18,2)",
            "double" => "FLOAT",
            "datetime" => "DATETIME2",
            "boolean" => "BIT",
            _ => "NVARCHAR(MAX)"
        };
    }

    private string SanitizeColumnName(string columnName)
    {
        // Remove or replace invalid characters
        var invalidChars = new[] { ' ', '\n', '\r', '\t', ',', '.', '/', '\\', '[', ']', '(', ')', '{', '}' };
        var sanitized = invalidChars.Aggregate(columnName, (current, c) => current.Replace(c, '_'));

        // Ensure the name starts with a letter
        if (!char.IsLetter(sanitized[0]))
            sanitized = "Col_" + sanitized;

        return sanitized;
    }

    private string SanitizeTableName(string tableName)
    {
        // Similar to column name sanitization but possibly with different rules
        var invalidChars = new[] { ' ', '\n', '\r', '\t', ',', '.', '/', '\\', '[', ']', '(', ')', '{', '}' };
        var sanitized = invalidChars.Aggregate(tableName, (current, c) => current.Replace(c, '_'));

        if (!char.IsLetter(sanitized[0]))
            sanitized = "Table_" + sanitized;

        return sanitized;
    }
}
