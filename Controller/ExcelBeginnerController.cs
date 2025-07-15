using ExcelReader.RyanW84.Data;
using ExcelReader.RyanW84.Models;
using ExcelReader.RyanW84.Services;

namespace ExcelReader.RyanW84.Controller;

public class ExcelBeginnerController
{
    public void AddDataFromExcel()
    {
        using var db = new ExcelReaderDbContext();

        var excelBeginners = ExcelBeginnerService.ReadExcelFile();
        db.AddRange(excelBeginners);
        db.SaveChanges();
    }
}