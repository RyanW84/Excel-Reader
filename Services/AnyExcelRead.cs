using System.Data;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;

namespace ExcelReader.RyanW84.Services;

public class AnyExcelRead(IConfiguration configuration)
{
    //private readonly ExcelReaderDbContext _context;

    // _context = new ExcelReaderDbContext(connectionString);

    public DataTable ReadFromExcel(string filePath)
    {
        ExcelPackage.License.SetNonCommercialPersonal("Ryan Weavers");
        using var package = new ExcelPackage(new FileInfo(filePath));
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

                    if (double.TryParse(cell.Text, out _))
                    {
                        readColumn.DataType = typeof(double);
                        typeDetected = true;
                        break;
                    }

                    if (int.TryParse(cell.Text, out _))
                    {
                        readColumn.DataType = typeof(int);
                        typeDetected = true;
                    }
                    else if (float.TryParse(cell.Text, out _))
                    {
                        readColumn.DataType = typeof(float);
                        typeDetected = true;
                    }
                    else if (DateTime.TryParse(cell.Text, out _))
                    {
                        readColumn.DataType = typeof(DateTime);
                        typeDetected = true;
                    }
                    else if (bool.TryParse(cell.Text, out _))
                    {
                        readColumn.DataType = typeof(bool);
                        typeDetected = true;
                    }
                }

            // If no other type was detected, set it to string
            if (typeDetected is not true) readColumn.DataType = typeof(string);
        }

        // Populate DataTable with Excel data
        for (var row = 2; row <= worksheet.Dimension.Rows; row++)
        {
            var dataRow = dataTable.NewRow();
            for (var col = 1; col <= worksheet.Dimension.Columns; col++)
            {
                var cellValue = worksheet.Cells[row, col].Text;
                dataRow[col - 1] = Convert.ChangeType(cellValue, dataTable.Columns[col - 1].DataType);
            }

            dataTable.Rows.Add(dataRow);
        }

        dataTable.TableName = worksheet.Name;
        return dataTable;
    }
}