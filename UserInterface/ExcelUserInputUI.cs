using System.Globalization;
using ExcelReader.RyanW84.Controller;
using ExcelReader.RyanW84.Services;
using Spectre.Console;

namespace ExcelReader.RyanW84.UI;

public class ExcelUserInputUI(ExcelWriteController controller , AnyExcelRead anyExcelRead)
{
	public ExcelWriteController Controller { get; } = controller;
	public AnyExcelRead AnyExcelRead { get; } = anyExcelRead;

	public void ExcelGatherInput(ExistingFields existingFields)
    {
        var filePath = AnsiConsole.Ask<string>(
            "\nEnter the path to the Excel file (or press Enter for default):",
            @"C:\Users\Ryanw\OneDrive\\Documents\GitHub\Excel-Reader\Data\\ExcelDynamic.xlsx"
        );

        existingFields = Controller.GetExistingFieldValues(filePath);
        if (existingFields.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]No fields found or file not found.[/]");
            return;
        }

        var fieldValues = new Dictionary<string, string>();
        string? dobValue = existingFields.TryGetValue("DOB" , out string? value) ? value : null;
        string? ageFieldName = null;

        AnsiConsole.MarkupLine("[yellow]Review and update Excel fields:[/]");
        foreach (var field in existingFields)
        {
            var fieldName = field.Key;
            var currentValue = field.Value;
            string newValue = currentValue;
            bool update = AnsiConsole.Confirm(
                $"Field: [green]{fieldName}[/] | Current Value: [yellow]{currentValue}[/] | Update?"
            );
            if (update)
            {
                if (fieldName.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    newValue = AnsiConsole.Prompt(
                        new TextPrompt<string>("Enter the updated Name:")
                            .DefaultValue(currentValue)
                            .AllowEmpty()
                    );
                }
                else if (fieldName.Equals("Surname", StringComparison.OrdinalIgnoreCase))
                {
                    newValue = AnsiConsole.Prompt(
                        new TextPrompt<string>("Enter the updated Surname:")
                            .DefaultValue(currentValue)
                            .AllowEmpty()
                    );
                }
                else if (
                    fieldName.Contains("dob" , StringComparison.CurrentCultureIgnoreCase)
				)
                {
                    newValue = AnsiConsole.Prompt(
                        new TextPrompt<string>("Enter Date of Birth (dd-MM-yyyy):").Validate(date =>
                            DateTime.TryParseExact(
                                date,
                                "dd-MM-yyyy",
                                CultureInfo.InvariantCulture,
                                DateTimeStyles.None,
                                out _
                            )
                                ? ValidationResult.Success()
                                : ValidationResult.Error("Invalid date format. Use dd-MM-yyyy.")
                        )
                    );
                    dobValue = newValue;
                }
                else if (fieldName.Equals("age", StringComparison.OrdinalIgnoreCase))
                {
                    AnsiConsole.MarkupLine("Age is autocalculated");
                    ageFieldName = fieldName;
                    continue;
                }
                else if (fieldName.Equals("sex", StringComparison.OrdinalIgnoreCase))
                {
                    newValue = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                            .Title("Select sex:")
                            .AddChoices("Male", "Female", "Other")
                    );
                }
                else if (fieldName.Equals("colour", StringComparison.OrdinalIgnoreCase))
                {
                    newValue = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                            .Title("Select colour:")
                            .AddChoices("White", "Black", "Asian", "African", "Other")
                    );
                }
            }
            fieldValues[fieldName] = newValue;
        }

        // After all fields, recalculate age from DOB if possible, case-insensitive
        ageFieldName ??= existingFields.Keys.FirstOrDefault(k => k.Equals("age", StringComparison.OrdinalIgnoreCase));
        if (ageFieldName != null)
        {
            if (
                !string.IsNullOrEmpty(dobValue)
                && DateTime.TryParseExact(
                    dobValue,
                    "dd-MM-yyyy",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out var dob
                )
            )
            {
                var today = DateTime.Today;
                var age = today.Year - dob.Year;
                if (dob > today.AddYears(-age))
                    age--;
                fieldValues[ageFieldName] = age.ToString();
                AnsiConsole.MarkupLine($"[green]Calculated age from DOB: {age}[/]");
            }
        }

        Controller.WriteDataToExcel(filePath, fieldValues);
    }
}
