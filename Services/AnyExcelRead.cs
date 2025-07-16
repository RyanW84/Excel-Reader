using System.Data;
using OfficeOpenXml;

namespace ExcelReader.RyanW84.Services;

public class AnyExcelRead
{
    [Obsolete("Obsolete")]
    public DataTable ReadFromExcel(string filePath)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        using var package = new ExcelPackage(new FileInfo("yourfile.xlsx"));
        var worksheet = package.Workbook.Worksheets[0]; // Get first worksheet
        var dataTable = new DataTable();

        for (var columns = 1; columns <= worksheet.Dimension.Columns; columns++)
        {
            var readColumn = dataTable.Columns.Add(worksheet.Cells[1, columns].Text);
            var typeDetected = false;

            foreach (var cell in worksheet.Cells[2, columns, worksheet.Dimension.Rows, columns])
                if (!string.IsNullOrEmpty(cell.Text))
                {
                    if (DateTime.TryParse(cell.Text, out _))
                    {
                        readColumn.DataType = typeof(DateTime);
                        typeDetected = true;
                        break;
                    }

                    else if (double.TryParse(cell.Text, out _))
                    {
                        readColumn.DataType = typeof(double);
                        typeDetected = true;
                        break;
                    }
                   else  if (int.TryParse(cell.Text, out _))
                        {
                        readColumn.DataType = typeof(int);
                        typeDetected = true;
                        }
                }

            // If no other type was detected, set it to string
            if (!typeDetected) readColumn.DataType = typeof(string);
        }

        return dataTable;
    }
}