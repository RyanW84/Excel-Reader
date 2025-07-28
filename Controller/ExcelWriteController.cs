using System.Data;
using ExcelReader.RyanW84.Data;
using ExcelReader.RyanW84.Services;
using Microsoft.Extensions.Configuration;
using ExcelReader.RyanW84.UserInterface;
using ExcelReader.RyanW84.Models;

namespace ExcelReader.RyanW84.Controller; //work on resolvoing user interface and making more modular

public class ExcelWriteController(IConfiguration configuration, ExcelReaderDbContext dbContext ,WriteToExcelService writeToExcelService , AnyExcelRead anyExcelRead, UserInterface userInterface)
{
    private readonly WriteToExcelService _writeToExcelService = writeToExcelService;
    private readonly AnyExcelRead _anyExcelRead = anyExcelRead;
    private readonly IConfiguration _configuration = configuration;
    private readonly ExcelReaderDbContext _dbContext = dbContext;
    private readonly UserInteFace _userInteFace = userInteFace;

	public void WriteDataToExcel(string filePath, Dictionary<string, string> fieldValues)
    {
        _writeToExcelService.WriteFieldsToExcel(filePath, fieldValues);
       
    }

    public Dictionary<string, string> GetExistingFieldValues(string filePath)
    {
        var table = _anyExcelRead.ReadFromExcel();
        var result = new Dictionary<string, string>();
        if (table.Rows.Count > 0)
        {
            foreach (DataColumn col in table.Columns)
            {
                result[col.ColumnName] = table.Rows[0][col].ToString() ?? string.Empty;
            }
        }
        return result;
    }

    public void GatherInputForExcelFields(ExistingFieldsAndValues existingFields)
    {
        var filePath = _userInteFace.GetFilePathFromUser(
            "Enter the path to the Excel file (or press Enter for default):",
            @"C:\Users\Ryanw\OneDrive\Documents\GitHub\Excel-Reader\Data\ExcelDynamic.xlsx"
        );
        existingFields = GetExistingFieldValues(filePath);
        if (existingFields is null)
			_userInterface.DisplayMessage("No fields found or file not found.");
            return;
        }
        var fieldValues = new Dictionary<string, string>();
        string? dobValue = existingFields.TryGetValue("DOB", out string? value) ? value : null;
        string? ageFieldName = null;
        _userInteFace.DisplayMessage("Review and update Excel fields:");
        foreach (var field in existingFields)
        {
            var fieldName = field.Key;
            var currentValue = field.Value;
            string newValue = currentValue;
            bool update = _userInteFace.ConfirmUpdate(fieldName, currentValue);
            if (update)
            {
                if (fieldName.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    newValue = _userInteFace.GetUpdatedFieldValue(fieldName, currentValue);
                }
                else if (fieldName.Equals("Surname", StringComparison.OrdinalIgnoreCase))
                {
                    newValue = _userInteFace.GetUpdatedFieldValue(fieldName, currentValue);
                }
                else if (fieldName.Contains("dob", StringComparison.CurrentCultureIgnoreCase))
                {
                    dobValue = _userInteFace.GetUpdatedDob(currentValue);
                    ageFieldName = "Age";
                }
                else
                {
                    newValue = _userInteFace.GetUpdatedFieldValue(fieldName, currentValue);
                }
            }
            fieldValues[fieldName] = newValue;
        }
        if (!string.IsNullOrEmpty(dobValue) && !string.IsNullOrEmpty(ageFieldName))
        {
            int age = CalculateAge(dobValue);
            fieldValues[ageFieldName] = age.ToString();
        }
        WriteDataToExcel(filePath, fieldValues);
    }
 

	public void WriteDataToDatabase(Dictionary<string, string> fieldValues)
    {
        var dataTable = new ConvertDictionaryToDataTable().Convert(fieldValues);
        _dbContext.Add(dataTable);
        _dbContext.SaveChanges();
		}
	}
