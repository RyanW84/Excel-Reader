using ExcelReader.RyanW84.Models;
using OfficeOpenXml;

namespace ExcelReader.RyanW84.Services;

public static class ExcelBeginnerService
{
    public static ExcelBeginner ReadExcelFile()
    {
        const string filePath = @"C:\Users\Ryanw\OneDrive\Documents\GitHub\Excel-Reader\Data\ExcelBeginner.xlsx";
        var file = new FileInfo(filePath);

        ExcelPackage.License.SetNonCommercialPersonal("Ryan Weavers");
        using (var package = new ExcelPackage(new FileInfo("ExcelBeginner.xlsx")))
        {
            ;


            var worksheet = package.Workbook.Worksheets["test"];

            var readName = worksheet.Cells["B2"];
            var readAge = worksheet.Cells["C2"];
            var readSex = worksheet.Cells["D2"];
            var readColour = worksheet.Cells["E2"];
            var readHeight = worksheet.Cells["F2"];


            var testRead = new ExcelBeginner()
            {
                Name = readName.ToString() ,
                age = int.Parse(readAge.ToString()) ,
                sex = readSex.ToString() ,
                colour = readColour.ToString() ,
                height = readHeight.ToString()
            };


            return testRead;
        }
    }
}