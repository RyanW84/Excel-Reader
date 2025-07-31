using System.Data;

using ExcelReader.RyanW84.Helpers;
using ExcelReader.RyanW84.Services;
using ExcelReader.RyanW84.UserInterface;

namespace ExcelReader.RyanW84.Controller;

public class ExcelWriteController
{
    private readonly WriteToExcelService _writeToExcelService;
    private readonly AnyExcelRead _anyExcelRead;
    private readonly ExcelUserInputUi _excelUserInputUi;
    private readonly WriteUpdatedExcelDataToDatabase _writeUpdatedExcelDataToDatabase;
    private readonly UserNotifier _userNotifier;

    public ExcelWriteController(
        WriteToExcelService writeToExcelService,
        AnyExcelRead anyExcelRead,
        ExcelUserInputUi excelUserInputUi,
        WriteUpdatedExcelDataToDatabase writeUpdatedExcelDataToDatabase,
        UserNotifier userNotifier
    )
    {
        _writeToExcelService = writeToExcelService;
        _anyExcelRead = anyExcelRead;
        _excelUserInputUi = excelUserInputUi;
        _writeUpdatedExcelDataToDatabase = writeUpdatedExcelDataToDatabase;
        _userNotifier = userNotifier;
    }

    // Orchestrator method for all steps
    public void UpdateExcelAndDatabase(string defaultPath)
    {
        // 1. Get file path from user
        var filePath = _excelUserInputUi.GetFilePath(defaultPath);

        // 2. Get existing field values from Excel
        var table = _anyExcelRead.ReadFromExcel();
        if (table == null || table.Rows.Count == 0)
        {
            _userNotifier.ShowError("No data found in the Excel file.");
            return;
        }
        var existingFields = new Dictionary<string, string>();
        foreach (DataColumn col in table.Columns)
        {
            existingFields[col.ColumnName] =
                table.Rows[0][col.ColumnName]?.ToString() ?? string.Empty;
        }

        // 3. Update field values interactively
        var updatedFields = _excelUserInputUi.UpdateFieldValues(existingFields);

        // 4. Write updated fields to Excel
        _writeToExcelService.WriteFieldsToExcel(filePath, updatedFields);

        // 5. Write updated fields to database
        _writeUpdatedExcelDataToDatabase.Write(updatedFields);

        _userNotifier.ShowSuccess("Excel file and database updated successfully!");
    }
}
