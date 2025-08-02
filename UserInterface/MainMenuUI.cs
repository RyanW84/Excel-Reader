// Ignore Spelling: pdf

using ExcelReader.RyanW84.Controller;
using ExcelReader.RyanW84.Services;
using Spectre.Console;

namespace ExcelReader.RyanW84.UI;

public class MainMenuUI(
    ExcelWriteController excelWriteController,
    AnyExcelRead anyExcelRead,
    PdfFormWriteController pdfFormWriteController,
    ReadFromPdfForm readFromPdfForm,
    CsvController csvController,
    AnyExcelReadController anyExcelReadController,
    ExcelBeginnerController excelBeginnerController,
    PdfController pdfController,
    PdfFormController pdfFormController
)
{
    private readonly ExcelWriteController _excelWriteController = excelWriteController;
    private readonly AnyExcelRead _anyExcelRead = anyExcelRead;
    private readonly PdfFormWriteController _pdfFormWriteController = pdfFormWriteController;
    private readonly ReadFromPdfForm _readFromPdfForm = readFromPdfForm;
    private readonly CsvController _csvController = csvController;
    private readonly AnyExcelReadController _anyExcelReadController = anyExcelReadController;
    private readonly ExcelBeginnerController _excelBeginnerController = excelBeginnerController;
    private readonly PdfController _pdfController = pdfController;
    private readonly PdfFormController _pdfFormController = pdfFormController;

    public async Task ShowMenuAsync()
    {
        Console.Clear();
        AnsiConsole.Write(new Rule("[yellow]FileRead[/]").RuleStyle("yellow").Centered());
        var exit = false;

        string excelFilePath =
            @"C:\Users\Ryanw\OneDrive\Documents\GitHub\Excel-Reader\Data\ExcelDynamic.xlsx";
        string pdfFilePath =
            @"C:\Users\Ryanw\OneDrive\Documents\GitHub\Excel-Reader\Data\FillablePDF.pdf";
        string csvFilePath =
            @"C:\Users\Ryanw\OneDrive\Documents\GitHub\Excel-Reader\Data\Sample.csv";

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
                    try
                    {
                        await _anyExcelReadController.AddDynamicDataFromExcel();
                    }
                    catch (Exception ex)
                    {
                        AnsiConsole.MarkupLine($"[red]Error during Excel import: {ex.Message}[/]");
                    }
                    break;
                case "Excel: Write":
                  
                    await _excelWriteController.UpdateExcelAndDatabaseAsync();
                    break;
                case "CSV: Import":
          
                    try
                    {
                        _csvController.AddDataFromCsv();
                    }
                    catch (Exception ex)
                    {
                        AnsiConsole.MarkupLine($"[red]Error during CSV import: {ex.Message}[/]");
                    }
                    break;
                case "PDF: Import":
                    try
                    {
                        _pdfController.AddDataFromPdf();
                    }
                    catch (Exception ex)
                    {
                        AnsiConsole.MarkupLine($"[red]Error during PDF import: {ex.Message}[/]");
                    }
                    break;
                case "PDF: Form Import":
  
                    try
                    {
                        _pdfFormController.AddOrUpdateDataFromPdfForm();
                    }
                    catch (Exception ex)
                    {
                        AnsiConsole.MarkupLine(
                            $"[red]Error during PDF form import: {ex.Message}[/]"
                        );
                    }
                    break;
                case "PDF: Form Write":
              
                    _pdfFormWriteController.UpdatePdfFormAndDatabase();
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
            : AnsiConsole.Ask<string>($"Enter the path to the {fileType}:", currentPath);
    }
}
