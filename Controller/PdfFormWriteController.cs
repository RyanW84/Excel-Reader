using ExcelReader.RyanW84.Data;
using ExcelReader.RyanW84.Services;
using Microsoft.Extensions.Configuration;
using ExcelReader.RyanW84.UserInterface;

namespace ExcelReader.RyanW84.Controller;

public class PdfFormWriteController(
    IConfiguration configuration,
    ExcelReaderDbContext dbContext,
    UserInterface userInterface ,
	WriteToPdfForm writeToPdfForm)
{
    private readonly IConfiguration _configuration = configuration;
    private readonly ExcelReaderDbContext _dbContext = dbContext;
    private readonly WriteToPdfForm _writeToPdfForm = writeToPdfForm;
    private readonly UserInterface _userInterface = userInterface;

	public void WriteDataToPdfForm(string filePath, Dictionary<string, string> fieldValues)
    {
        Console.WriteLine($"Writing data to PDF form: {filePath}");
        _writeToPdfForm.WriteFormFields(filePath, fieldValues);
        Console.WriteLine("PDF form fields updated.");
        _dbContext.SaveChanges();
    }

    public Dictionary<string, string> GetExistingFieldValues(string filePath)
    {
        Console.WriteLine($"Reading existing field values from PDF form: {filePath}");
        var existingFields = _writeToPdfForm.ReadFormFields(filePath);
        var result = new Dictionary<string, string>();
        foreach (var field in existingFields)
        {
            result[field.Key] = field.Value ?? string.Empty;
        }
        return result;
		}






	public void WriteDataToDatabase(Dictionary<string, string> fieldValues)
    {
        var dataTable = new ConvertDictionaryToDataTable().Convert(fieldValues);
        _dbContext.Add(dataTable);
        _dbContext.SaveChanges();
		}
	}