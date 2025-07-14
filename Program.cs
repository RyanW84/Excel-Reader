using ExcelReader.RyanW84.Data;
using ExcelReader.RyanW84.Controller;

var context = new ExcelReaderDbContext();
context.Database.EnsureDeleted();
context.Database.EnsureCreated();

ExcelBeginnerController excelBeginnerController = new();
excelBeginnerController.AddDataFromExcel();

