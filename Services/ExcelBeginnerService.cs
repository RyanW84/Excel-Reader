using ExcelReader.RyanW84.Models;
using OfficeOpenXml;

namespace ExcelReader.RyanW84.Services;

public static class ExcelBeginnerService
{
    public static (List<string> Headers, List<ExcelBeginner> Data) ReadExcelFile()
    {
        const string filePath =
            @"C:\Users\Ryanw\OneDrive\Documents\GitHub\Excel-Reader\Data\ExcelBeginner.xlsx";
        var file = new FileInfo(filePath);

        if (!file.Exists)
        {
            throw new FileNotFoundException($"Excel file not found at path: {filePath}");
        }

        ExcelPackage.License.SetNonCommercialPersonal("Ryan Weavers");
        using (var package = new ExcelPackage(file))
        {
            var worksheet = package.Workbook.Worksheets["test"];
            
            if (worksheet == null)
            {
                throw new Exception("Worksheet 'test' not found in the Excel file");
            }

            // Get the dimensions of the worksheet
            int rowCount = worksheet.Dimension.Rows;
            int colCount = worksheet.Dimension.Columns;

            // Read headers (first row)
            var headers = new List<string>();
            for (int col = 1; col <= colCount; col++)
            {
                var headerText = worksheet.Cells[1, col].Text;
                headers.Add(string.IsNullOrEmpty(headerText) ? $"Column{col}" : headerText);
            }

            // Print headers for verification
            Console.WriteLine("Headers found:");
            foreach (var header in headers)
            {
                Console.WriteLine($"- {header}");
            }

            var results = new List<ExcelBeginner>();
            
            // Start from row 2 (data rows)
            for (int row = 2; row <= rowCount; row++)
            {
                var readName = worksheet.Cells[row, 2].Text;      // Column B
                var readAge = worksheet.Cells[row, 3].Text;       // Column C
                var readSex = worksheet.Cells[row, 4].Text;       // Column D
                var readColour = worksheet.Cells[row, 5].Text;    // Column E
                var readHeight = worksheet.Cells[row, 6].Text;    // Column F

                // Skip empty rows
                if (string.IsNullOrWhiteSpace(readName))
                {
                    continue;
                }

                if (!int.TryParse(readAge, out int age))
                {
                    throw new Exception($"Invalid age value in cell C{row}");
                }

                var excelBeginner = new ExcelBeginner()
                {
                    Name = readName,
                    age = age,
                    sex = readSex ?? string.Empty,
                    colour = readColour ?? string.Empty,
                    height = readHeight ?? string.Empty
                };

                results.Add(excelBeginner);
            }

            return (headers, results);
        }
    }
}