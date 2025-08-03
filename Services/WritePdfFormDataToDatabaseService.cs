using System.Data;
using Microsoft.Extensions.Configuration;

namespace ExcelReader.RyanW84.Services;

public class WritePdfFormDataToDatabaseService(IConfiguration configuration , CreateTableFromPdfForm createTableFromPdfForm)
{
    private readonly IConfiguration _configuration = configuration;
    private readonly CreateTableFromPdfForm _createTableFromPdfForm = createTableFromPdfForm;

	public async Task WriteAsync(Dictionary<string , string> fieldValues)
	{
		var dataTable = new DataTable("PdfFormData");
		foreach (var key in fieldValues.Keys)
		{
			dataTable.Columns.Add(key);
		}
		var row = dataTable.NewRow();
		foreach (var kvp in fieldValues)
		{
			row[kvp.Key] = kvp.Value ?? string.Empty;
		}
		dataTable.Rows.Add(row);

		// Offload the database operation to a background thread
		await _createTableFromPdfForm.CreateTableFromPdfFormData(dataTable);

	
	}
}
