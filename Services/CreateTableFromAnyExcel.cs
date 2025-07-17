using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace ExcelReader.RyanW84.Services;

public class CreateTableFromAnyExcel(IConfiguration configuration)
{
    public void CreateTableFromExcel(DataTable dataTable)
    {
        var tableName = $"{dataTable.TableName}";
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        using var connection = new SqlConnection(connectionString);
        connection.Open();
        var sqlScript = $"CREATE TABLE {tableName} (\n";
        foreach (DataColumn column in dataTable.Columns)
        {
            var sqlDataType = GetSqlDataType(column.DataType);
            sqlScript += $"    {column.ColumnName} {sqlDataType},\n";
        }

        sqlScript = sqlScript.Remove(sqlScript.Length - 2);
        sqlScript += ")";
        using var command = new SqlCommand(sqlScript, connection);
        command.ExecuteNonQuery();
        using var bulkCopy = new SqlBulkCopy(connection);
        bulkCopy.DestinationTableName = tableName;
        bulkCopy.WriteToServer(dataTable);
    }

    private static string GetSqlDataType(Type type)
    {
        return type.Name switch
        {
            "String" => "nvarchar(max)",
            "Int32" => "int",
            "DateTime" => "datetime",
            "Double" => "float",
            "Boolean" => "bit",
            _ => "nvarchar(max)"
        };
    }
}