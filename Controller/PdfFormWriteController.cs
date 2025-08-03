using System.Data;
using System.Data.SqlClient;
using ExcelReader.RyanW84.Data;
using ExcelReader.RyanW84.Services;
using ExcelReader.RyanW84.UserInterface;
using Microsoft.Extensions.Configuration;
using ExcelReader.RyanW84.Abstractions;
using ExcelReader.RyanW84.Helpers;

namespace ExcelReader.RyanW84.Controller;

public class PdfFormWriteController(
    IConfiguration configuration,
    ExcelReaderDbContext dbContext,
    IPdfFormReader readFromPdfForm,
    IPdfFormWriter writeToPdfForm,
    IPdfFormDatabaseService writePdfFormDataToDatabaseService,
    IFieldInputService fieldInputUi,
    ITableManager tableExistenceService,
    INotificationService userNotifier,
    IFilePathService filePathManager
)
{
    private readonly IConfiguration _configuration = configuration;
    private readonly ExcelReaderDbContext _dbContext = dbContext;
    private readonly IPdfFormWriter _writeToPdfForm = writeToPdfForm;
    private readonly IPdfFormReader _readFromPdfForm = readFromPdfForm;
    private readonly IPdfFormDatabaseService _writePdfFormDataToDatabaseService =
        writePdfFormDataToDatabaseService;
    private readonly IFieldInputService _fieldInputUi = fieldInputUi;
    private readonly ITableManager _tableExistenceService = tableExistenceService;
    private readonly INotificationService _userNotifier = userNotifier;
    private readonly IFilePathService _filePathManager = filePathManager;

    // Orchestrator method for all PDF form write steps
    public async Task UpdatePdfFormAndDatabaseAsync()
    {
        try
        {
			// 1. Get file path from user via FilePathManager
			var customDefault = @"C:\\Users\\Ryanw\\OneDrive\\Documents\\GitHub\\Excel-Reader\\Data\\TablePDF.pdf";
			var filePath = _filePathManager.GetFilePath(FileType.PDF, customDefault);

			// 2. Get existing field values from PDF
			var existingFields = GetExistingFieldValues(filePath);
            if (existingFields.Count == 0)
            {
                _userNotifier.ShowError("No form fields found or file not found.");
                return;
            }

            // 3. Get updated field values from user interaction using unified UI
            var updatedFields = GetUpdatedFieldValues(existingFields);

            // 4. Write updated fields to PDF form
            await WriteDataToPdfFormAsync(filePath, updatedFields);

            // 5. Write updated fields to database
            await WriteDataToDatabaseAsync(updatedFields);

            _userNotifier.ShowSuccess("PDF form and database updated successfully!");
        }
        catch (FilePathValidationException ex)
        {
            _userNotifier.ShowError($"File path error: {ex.Message}");
        }
        catch (FileNotFoundException ex)
        {
            _userNotifier.ShowError($"PDF file not found: {ex.Message}");
        }
        catch (UnauthorizedAccessException ex)
        {
            _userNotifier.ShowError($"Access denied: {ex.Message}. Please check file permissions.");
        }
        catch (IOException ex)
        {
            _userNotifier.ShowError($"File I/O error: {ex.Message}. The PDF file may be in use by another application.");
        }
        catch (Exception ex)
        {
            _userNotifier.ShowError($"An unexpected error occurred: {ex.Message}");
        }
    }

    // Keep synchronous version for backward compatibility
    public void UpdatePdfFormAndDatabase(string filePath)
    {
        try
        {
            // Legacy method that accepts file path parameter
            // 1. Get existing field values from PDF
            var existingFields = GetExistingFieldValues(filePath);
            if (existingFields.Count == 0)
            {
                _userNotifier.ShowError("No form fields found or file not found.");
                return;
            }

            // 2. Get updated field values from user interaction
            var updatedFields = GetUpdatedFieldValues(existingFields);

            // 3. Write updated fields to PDF form
            WriteDataToPdfFormAsync(filePath, updatedFields).GetAwaiter().GetResult();

            // 4. Write updated fields to database
            WriteDataToDatabaseAsync(updatedFields).GetAwaiter().GetResult();

            _userNotifier.ShowSuccess("PDF form and database updated successfully!");
        }
        catch (FilePathValidationException ex)
        {
            _userNotifier.ShowError($"File path error: {ex.Message}");
        }
        catch (Exception ex)
        {
            _userNotifier.ShowError($"An unexpected error occurred: {ex.Message}");
        }
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

    public Dictionary<string, string> GetUpdatedFieldValues(Dictionary<string, string> fieldValues)
    {
        return _fieldInputUi.GatherUpdatedFields(fieldValues, FileType.PDF);
    }

    public async Task WriteDataToPdfFormAsync(string filePath, Dictionary<string, string> fieldValues)
    {
        await _writeToPdfForm.WriteFormFieldsAsync(filePath, fieldValues);
        await _dbContext.SaveChangesAsync();
    }

    public async Task WriteDataToDatabaseAsync(Dictionary<string, string> fieldValues)
    {
        await _writePdfFormDataToDatabaseService.WriteAsync(fieldValues);
    }

    // Legacy synchronous methods for backward compatibility
    public void WriteDataToPdfForm(string filePath, Dictionary<string, string> fieldValues)
    {
        WriteDataToPdfFormAsync(filePath, fieldValues).GetAwaiter().GetResult();
    }

    public void WriteDataToDatabase(Dictionary<string, string> fieldValues)
    {
        WriteDataToDatabaseAsync(fieldValues).GetAwaiter().GetResult();
    }

    public void EnsureTableExists(DataTable dataTable, SqlConnection connection)
    {
        _tableExistenceService.EnsureTableExists(dataTable, connection);
    }

    public void EnsureTableExists(DataTable dataTable)
    {
        _tableExistenceService.EnsureTableExists(dataTable);
    }
}
