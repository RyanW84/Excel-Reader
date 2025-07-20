using System.Data;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;

namespace ExcelReader.RyanW84.Services;

public class AnyExcelRead(IConfiguration configuration)
{
    public DataTable ReadFromExcel()
    {
        var HasHeader = true;
        
        Console.WriteLine($"\nReading from a dynamic table");

        const string filePath = @"C:\Users\Ryanw\OneDrive\Documents\GitHub\Excel-Reader\Data\ExcelDynamic.xlsx";
        Console.WriteLine($"opening {filePath}");

        ExcelPackage.License.SetNonCommercialPersonal("Ryan Weavers");

        using var package = new ExcelPackage(new FileInfo(filePath));

        var worksheet = package.Workbook.Worksheets[0]; // Get first worksheet
        Console.WriteLine($"Worksheet found: {worksheet}");
        var dataTable = new DataTable();

        for (var columns = 1; columns <= worksheet.Dimension.Columns; columns++)
        {
            var readColumn = dataTable.Columns.Add(worksheet.Cells[1, columns].Text);
            var typeDetected = false; 
            Console.WriteLine($"column added: {readColumn.ColumnName}");

            foreach (var cell in worksheet.Cells[2, columns, worksheet.Dimension.Rows, columns])
                if (!string.IsNullOrEmpty(cell.Text))
                {
                    if (DateTime.TryParse(cell.Text, out _))
                    {
                        Console.WriteLine("Detected DateTime Data Type");
                        readColumn.DataType = typeof(DateTime);
                        typeDetected = true;
                        break;
                    }

                    if (double.TryParse(cell.Text, out _))
                    {
                        Console.WriteLine("Detected \"Double\" Data type");
                        readColumn.DataType = typeof(double);
                        typeDetected = true;
                        break;
                    }

                    if (int.TryParse(cell.Text, out _))
                    {
                        Console.WriteLine("Detected \"Int\" Data type");
                        readColumn.DataType = typeof(int);
                        typeDetected = true;
                    }
                    else if (float.TryParse(cell.Text, out _))
                    {
                        Console.WriteLine("Detected \"Float\" Data type");
                        readColumn.DataType = typeof(float);
                        typeDetected = true;
                    }
                    else if (bool.TryParse(cell.Text, out _))
                    {
                        Console.WriteLine("Detected \"Bool\" Data type");
                        readColumn.DataType = typeof(bool);
                        typeDetected = true;
                    }
                }

            // If no other type was detected, set it to string
            if (typeDetected is not true)
            {
                Console.WriteLine("Type not detected, defaulting to string");
                readColumn.DataType = typeof(string);
            }
        }

        // Populate DataTable with Excel data
        for (var row = 2; row <= worksheet.Dimension.Rows; row++)
        {
            Console.WriteLine($"Row added: {row} ");
            var dataRow = dataTable.NewRow();
            for (var col = 1; col <= worksheet.Dimension.Columns; col++)
            {
                var cellValue = worksheet.Cells[row, col].Text;
                dataRow[col - 1] = Convert.ChangeType(
                    cellValue,
                    dataTable.Columns[col - 1].DataType);
                Console.WriteLine($"Cell value: {cellValue} ");
            }

            dataTable.Rows.Add(dataRow);
        }

        dataTable.TableName = worksheet.Name;
        return dataTable;
    }
}