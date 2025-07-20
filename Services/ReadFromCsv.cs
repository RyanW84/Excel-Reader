using System.Data;
using System.IO;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;

namespace ExcelReader.RyanW84.Services;

public class ReadFromCsv(IConfiguration configuration)
{
    public List<string[]> ReadCsvFile()
    {
        string filePath =
            @"C:\Users\Ryanw\OneDrive\Documents\GitHub\Excel-Reader\Data\ExcelCSV.csv";
        Console.WriteLine($"opening {filePath}");

        ExcelPackage.License.SetNonCommercialPersonal("Ryan Weavers");

        var csvData = new List<string[]>();

        using var package = new ExcelPackage();
        using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        using var reader = new StreamReader(stream);
        var csvContent = reader.ReadToEnd();

        // Load CSV into worksheet
        var worksheet = package.Workbook.Worksheets.Add("CSV");
        worksheet.Cells.LoadFromText(csvContent);

        var rowCount = worksheet.Dimension.Rows;
        var colCount = worksheet.Dimension.Columns;

        for (var row = 1; row <= rowCount; row++)
        {
            var rowData = new string[colCount];
            for (var col = 1; col <= colCount; col++)
            {
                rowData[col - 1] = worksheet.Cells[row, col].Text;
            }
            csvData.Add(rowData);
        }

        return csvData;
    }

    public DataTable ConvertToDataTable(List<string[]> csvData)
    {
        var dataTable = new DataTable();

        if (csvData == null || csvData.Count == 0)
            return dataTable;

        // Add columns using the first row as header
        foreach (var columnName in csvData[0])
        {
            dataTable.Columns.Add(columnName ?? string.Empty);
        }

        // Add rows (skip header row)
        for (int i = 1; i < csvData.Count; i++)
        {
            dataTable.Rows.Add(csvData[i]);
        }

        return dataTable;
    }
}
