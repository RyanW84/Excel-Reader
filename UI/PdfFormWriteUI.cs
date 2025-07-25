using System.Globalization;
using ExcelReader.RyanW84.Controller;
using ExcelReader.RyanW84.Services;
using Spectre.Console;

namespace ExcelReader.RyanW84.UI;

public class PdfFormWriteUI(PdfFormWriteController controller , ReadFromPdfForm readFromPdfForm)
{
	public void PdfGatherInput()
    {
        var filePath = AnsiConsole.Ask<string>(
            "\nEnter the path to the PDF form file (or press Enter for default):",
            @"C:\Users\Ryanw\OneDrive\Documents\GitHub\Excel-Reader\Data\FillablePDF.pdf"
        );

        var fields = readFromPdfForm.ReadFormFields(filePath);
        if (fields.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]No form fields found or file not found.[/]");
            return;
        }

        var fieldValues = new Dictionary<string, string>();
        string? dobValue = fields.TryGetValue("DOB" , out string? value) ? value : null;
        string? ageFieldName = null;

        AnsiConsole.MarkupLine("[yellow]Review and update PDF form fields:[/]");
        foreach (var field in fields)
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
                    fieldName.Equals("DOB", StringComparison.OrdinalIgnoreCase)
                    || fieldName.Contains("dob" , StringComparison.CurrentCultureIgnoreCase)
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
                else if (fieldName.Equals("wanted", StringComparison.OrdinalIgnoreCase))
                {
                    newValue = AnsiConsole.Prompt(
                        new SelectionPrompt<string>().Title("Is wanted?").AddChoices("Yes", "No")
                    );
                }
            }
            fieldValues[fieldName] = newValue;
        }

        // After all fields, recalculate age from DOB if possible, case-insensitive
        ageFieldName ??= fields.Keys.FirstOrDefault(k => k.Equals("age", StringComparison.OrdinalIgnoreCase));
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

        controller.WriteDataToPdfForm(filePath, fieldValues);
    }
}
