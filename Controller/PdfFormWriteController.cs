using ExcelReader.RyanW84.Data;
using ExcelReader.RyanW84.Services;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;

namespace ExcelReader.RyanW84.Controller;

public class PdfFormWriteController(
    IConfiguration configuration,
    ExcelReaderDbContext dbContext,
    ReadFromPdfForm readFromPdfForm,
    WriteToPdfForm writeToPdfForm,
    WritePdfFormDataToDatabaseService writePdfFormDataToDatabaseService)
{
    private readonly IConfiguration _configuration = configuration;
    private readonly ExcelReaderDbContext _dbContext = dbContext;
    private readonly WriteToPdfForm _writeToPdfForm = writeToPdfForm;
    private readonly ReadFromPdfForm _readFromPdfForm = readFromPdfForm;
    private readonly WritePdfFormDataToDatabaseService _writePdfFormDataToDatabaseService = writePdfFormDataToDatabaseService;

    public void WriteDataToPdfForm(string filePath, Dictionary<string, string> fieldValues)
    {
        _writeToPdfForm.WriteFormFields(filePath, fieldValues);
        _dbContext.SaveChanges();
    }

    public Dictionary<string, string> GetExistingFieldValues(string filePath)
    {
        var existingFields = _readFromPdfForm.ReadFormFields(filePath);
        var result = new Dictionary<string, string>();
        foreach (var field in existingFields)
        {
            result[field.Key] = field.Value ?? string.Empty;
        }
        return result;
    }

    public void WriteDataToDatabase(Dictionary<string, string> fieldValues)
    {
        _writePdfFormDataToDatabaseService.Write(fieldValues);
    }
}