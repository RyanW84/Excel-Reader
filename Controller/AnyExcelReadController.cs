using ExcelReader.RyanW84.Data;
using ExcelReader.RyanW84.Services;
using Microsoft.Extensions.Configuration;

namespace ExcelReader.RyanW84.Controller;

public class AnyExcelReadController
{
    private readonly IConfiguration _configuration;
    private readonly ExcelReaderDbContext _dbContext;
    private readonly AnyExcelRead _anyExcelRead;
    private readonly CreateTableFromAnyExcel _createTableFromAnyExcel;

    public AnyExcelReadController(IConfiguration configuration, ExcelReaderDbContext dbContext, AnyExcelRead anyExcelRead, CreateTableFromAnyExcel createTableFromAnyExcel)
    {
        _configuration = configuration;
        _dbContext = dbContext;
        _anyExcelRead = anyExcelRead;
        _createTableFromAnyExcel = createTableFromAnyExcel;
    }

    public void AddDynamicDataFromExcel()
    {
        var dataTable = _anyExcelRead.ReadFromExcel();
        _createTableFromAnyExcel.CreateTableFromExcel(dataTable);
        _dbContext.SaveChanges();
    }
}