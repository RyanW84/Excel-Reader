using System.Data;
using ExcelReader.RyanW84.Data;
using ExcelReader.RyanW84.Models;
using ExcelReader.RyanW84.Services;
using Microsoft.Extensions.Configuration;

namespace ExcelReader.RyanW84.Controller;

public class ExcelBeginnerController(IConfiguration configuration, ExcelReaderDbContext dbContext)
{
    private readonly IConfiguration _configuration = configuration;

    // Inject both IConfiguration and ExcelReaderDbContext

    public void AddDataFromExcel()
    {
        Console.WriteLine("Starting ExcelBeginner import...");
        using var db = dbContext;
        var excelPackage = ExcelBeginnerService.ExcelPackage();
        var dataTable = ExcelBeginnerService.ReadFromExcel(excelPackage);
        Console.WriteLine($"Read {dataTable.Rows.Count} Rows from ExcelBeginner sheet.");
		Console.WriteLine($"Read {dataTable.Columns.Count} Columns from ExcelBeginner sheet.");

		var excelBeginners = ConvertDataTableToExcelBeginners(dataTable);
        db.ExcelBeginner.AddRange(excelBeginners);

        db.SaveChanges();
        Console.WriteLine("ExcelBeginner import complete.");
        excelPackage.Dispose();
	}

	private List<ExcelBeginner> ConvertDataTableToExcelBeginners(DataTable dataTable)
    {
        var excelBeginners = new List<ExcelBeginner>();

        foreach (DataRow row in dataTable.Rows)
        {
            var excelBeginner = new ExcelBeginner
            {
                Name = row["Name"].ToString() ?? string.Empty,
                age = int.TryParse(row["age"].ToString(), out var age) ? age : 0,
                sex = row["sex"].ToString() ?? string.Empty,
                colour = row["colour"].ToString() ?? string.Empty,
                height = row["height"].ToString() ?? string.Empty
            };

            excelBeginners.Add(excelBeginner);
        }

        return excelBeginners;
    }
}