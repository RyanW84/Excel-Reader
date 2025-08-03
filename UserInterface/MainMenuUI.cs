// Ignore Spelling: pdf

using ExcelReader.RyanW84.Controller;
using ExcelReader.RyanW84.Services;
using ExcelReader.RyanW84.Abstractions;
using Spectre.Console;

namespace ExcelReader.RyanW84.UserInterface;

public class MainMenuUI(
    ExcelWriteController excelWriteController,
    IAnyExcelReader anyExcelRead,
    PdfFormWriteController pdfFormWriteController,
    IPdfFormReader readFromPdfForm,
    CsvController csvController,
    AnyExcelReadController anyExcelReadController,
    ExcelBeginnerController excelBeginnerController,
    PdfTableController pdfController,
    PdfFormController pdfFormController
)
{
    private readonly ExcelWriteController _excelWriteController = excelWriteController;
    private readonly IAnyExcelReader _anyExcelRead = anyExcelRead;
    private readonly PdfFormWriteController _pdfFormWriteController = pdfFormWriteController;
    private readonly IPdfFormReader _readFromPdfForm = readFromPdfForm;
    private readonly CsvController _csvController = csvController;
    private readonly AnyExcelReadController _anyExcelReadController = anyExcelReadController;
    private readonly ExcelBeginnerController _excelBeginnerController = excelBeginnerController;
    private readonly PdfTableController _pdfController = pdfController;
    private readonly PdfFormController _pdfFormController = pdfFormController;

    public async Task ShowMenuAsync()
    {
        Console.Clear();
        AnsiConsole.Write(new Rule("[yellow]FileRead[/]").RuleStyle("yellow").Centered());
        var exit = false;

        while (!exit)
        {
            AnsiConsole.Write(new Rule("[yellow]FileRead[/]").RuleStyle("yellow").Centered());
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold yellow]Select an operation:[/]")
                    .AddChoices(
                        [
                            "Excel: Beginner Import",
                            "Excel: Dynamic Import",
                            "Excel: Write",
                            "CSV: Import",
                            "PDF: Import",
                            "PDF: Form Import",
                            "PDF: Form Write",
                            "Exit",
                        ]
                    )
            );

            switch (choice)
            {
                case "Excel: Beginner Import":
                    await _excelBeginnerController.AddDataFromExcel();
                    break;
                case "Excel: Dynamic Import":
                    await _anyExcelReadController.AddDynamicDataFromExcel();
                    break;
                case "Excel: Write":
                    await _excelWriteController.UpdateExcelAndDatabaseAsync();
                    break;
                case "CSV: Import":
                    await _csvController.AddDataFromCsv();
                    break;
                case "PDF: Import":
                    await _pdfController.AddDataFromPdf();
                    break;
                case "PDF: Form Import":
                    await _pdfFormController.AddOrUpdateDataFromPdfForm();
                    break;
                case "PDF: Form Write":
                    await _pdfFormWriteController.UpdatePdfFormAndDatabaseAsync();
                    break;
                case "Exit":
                    exit = true;
                    break;
            }
        }
    }

    private string PromptForFilePath(string currentPath, string fileType)
    {
        var useExisting = AnsiConsole.Confirm(
            $"Use existing {fileType} path? [green]{currentPath}[/]"
        );
        return useExisting
            ? currentPath
            : AnsiConsole.Ask($"Enter the path to the {fileType}:", currentPath);
    }
}
