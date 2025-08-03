using ExcelReader.RyanW84.Abstractions;
using OfficeOpenXml;

namespace ExcelReader.RyanW84.Services;

public class WriteToExcelService : IExcelWriteService
{
    public void WriteFieldsToExcel(string filePath, Dictionary<string, string> fieldValues)
    {
        // Set the license using the new EPPlus 8+ API
        ExcelPackage.License.SetNonCommercialPersonal("Ryan Weavers");

        using (var package = new ExcelPackage(new FileInfo(filePath)))
        {
            ExcelWorksheet worksheet;
            if (package.Workbook.Worksheets.Count == 0)
                worksheet = package.Workbook.Worksheets.Add("Sheet1");
            else
                worksheet = package.Workbook.Worksheets[0];

            int col = 1;
            foreach (var kvp in fieldValues)
            {
                worksheet.Cells[1, col].Value = kvp.Key;
                worksheet.Cells[2, col].Value = kvp.Value;
                col++;
            }
            package.Save();
        }
    }
}
