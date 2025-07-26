using ExcelReader.RyanW84.Data;
using ExcelReader.RyanW84.Services;
using Microsoft.Extensions.Configuration;

namespace ExcelReader.RyanW84.Controller;

public class PdfFormWriteController(
    IConfiguration configuration,
    ExcelReaderDbContext dbContext,
    WriteToPdfForm writeToPdfForm)
{
    private readonly IConfiguration _configuration = configuration;
    private readonly ExcelReaderDbContext _dbContext = dbContext;
    private readonly WriteToPdfForm _writeToPdfForm = writeToPdfForm;

    public void WriteDataToPdfForm(string filePath, Dictionary<string, string> fieldValues)
    {
        Console.WriteLine($"Writing data to PDF form: {filePath}");
        _writeToPdfForm.WriteFormFields(filePath, fieldValues);
        Console.WriteLine("PDF form fields updated.");
        _dbContext.SaveChanges();
    }
}