using System.Data;
using ExcelReader.RyanW84.Abstractions.Data.TableCreators;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace ExcelReader.RyanW84.Helpers;

public class CreateTableFromAnyExcel(IConfiguration configuration) : IExcelTableCreator
{
    private readonly IConfiguration _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

	public void CreateTableFromExcel(DataTable dataTable)
    {
        ArgumentNullException.ThrowIfNull(dataTable);

        var connectionString =
            _configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("DefaultConnection string not found");

        using var connection = new SqlConnection(connectionString);
        connection.Open();

        var createTableSql = SqlDataTypeMapper.BuildCreateTableStatement(
            dataTable.TableName,
            dataTable.Columns
        );

        using var command = new SqlCommand(createTableSql, connection);
        command.ExecuteNonQuery();

        using var bulkCopy = new SqlBulkCopy(connection);
        bulkCopy.DestinationTableName = SqlDataTypeMapper.SanitizeTableName(dataTable.TableName);
        bulkCopy.WriteToServer(dataTable);
    }
}
