using System.Data;
using ExcelReader.RyanW84.Helpers;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;

namespace ExcelReader.RyanW84.Services;

public class AnyExcelRead(IConfiguration configuration, UserNotifier userNotifier)
{
    private readonly IConfiguration _configuration = configuration;
    private readonly UserNotifier _userNotifier = userNotifier;

    public async Task<DataTable> ReadFromExcelAsync()
    {
        const string filePath = @"C:\Users\Ryanw\OneDrive\Documents\GitHub\Excel-Reader\Data\ExcelDynamic.xlsx";

        return await Task.Run(() =>
        {
            ExcelPackage.License.SetNonCommercialPersonal("Ryan Weavers");

            using var package = new ExcelPackage(new FileInfo(filePath));
            var worksheet = package.Workbook.Worksheets[0]; // Get first worksheet

            var dataTable = new DataTable();

            // Add columns with type detection
            for (var columns = 1; columns <= worksheet.Dimension.Columns; columns++)
            {
                var readColumn = dataTable.Columns.Add(worksheet.Cells[1, columns].Text);
                var typeDetected = false;
                Console.WriteLine($"column added: {readColumn.ColumnName}");

                foreach (var cell in worksheet.Cells[2, columns, worksheet.Dimension.Rows, columns])
                    if (!string.IsNullOrEmpty(cell.Text))
                    {
                        if (DateTime.TryParse(cell.Text, out var dateValue))
                        {
                            readColumn.DataType = typeof(string); // Store as string in dd-MM-yyyy format
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
                        else if (bool.TryParse(cell.Text, out _))
                        {
                            readColumn.DataType = typeof(bool);
                            typeDetected = true;
                        }
                    }

                // If no other type was detected, set it to string
                if (typeDetected is not true)
                {
                    readColumn.DataType = typeof(string);
                }
            }

            // Populate DataTable with Excel data
            for (var row = 2; row <= worksheet.Dimension.Rows; row++)
            {
                var dataRow = dataTable.NewRow();
                for (var col = 1; col <= worksheet.Dimension.Columns; col++)
                {
                    var cellValue = worksheet.Cells[row, col].Text;
                    var colType = dataTable.Columns[col - 1].DataType;
                    if (colType == typeof(string) && DateTime.TryParse(cellValue, out var dateValue))
                    {
                        dataRow[col - 1] = dateValue.ToString("dd-MM-yyyy"); // Store as date-only string
                    }
                    else
                    {
                        dataRow[col - 1] = Convert.ChangeType(cellValue, colType);
                    }
                }

                dataTable.Rows.Add(dataRow);
            }

            dataTable.TableName = worksheet.Name;
            return dataTable;
        });
    }

    // Keep synchronous version for backward compatibility
    public DataTable ReadFromExcel()
    {
        return ReadFromExcelAsync().GetAwaiter().GetResult();
    }
}
