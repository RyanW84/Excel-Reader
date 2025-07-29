using System.Data;
using Microsoft.Extensions.Configuration;

namespace ExcelReader.RyanW84.Services;

public class WritePdfFormDataToDatabaseService
{
    private readonly IConfiguration _configuration;
    private readonly CreateTableFromPdfForm _createTableFromPdfForm;

    public WritePdfFormDataToDatabaseService(IConfiguration configuration, CreateTableFromPdfForm createTableFromPdfForm)
    {
        _configuration = configuration;
        _createTableFromPdfForm = createTableFromPdfForm;
    }

    public void Write(Dictionary<string, string> fieldValues)
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
        _createTableFromPdfForm.CreateTableFromPdfFormData(dataTable);
    }
}
