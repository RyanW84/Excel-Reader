// Ignore Spelling: pdf

using ExcelReader.RyanW84.Controller;
using ExcelReader.RyanW84.Services;
using Spectre.Console;

namespace ExcelReader.RyanW84.UI;

public class MainMenuUI
{
    private readonly ExcelWriteController _excelWriteController;
    private readonly AnyExcelRead _anyExcelRead;
    private readonly PdfFormWriteController _pdfFormWriteController;
    private readonly ReadFromPdfForm _readFromPdfForm;
    private readonly CsvController _csvController;
    private readonly AnyExcelReadController _anyExcelReadController;
    private readonly ExcelBeginnerController _excelBeginnerController;
    private readonly PdfController _pdfController;
    private readonly PdfFormController _pdfFormController;

    public MainMenuUI(
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
        _excelWriteController = excelWriteController;
        _anyExcelRead = anyExcelRead;
        _pdfFormWriteController = pdfFormWriteController;
        _readFromPdfForm = readFromPdfForm;
        _csvController = csvController;
        _anyExcelReadController = anyExcelReadController;
        _excelBeginnerController = excelBeginnerController;
        _pdfController = pdfController;
        _pdfFormController = pdfFormController;
    }

    public void ShowMenu()
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
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold yellow]Select an operation:[/]")
                    .AddChoices(
                        new[]
                        {
                            "Excel: Beginner Import",
                            "Excel: Dynamic Import",
                            "CSV: Import",
                            "PDF: Import",
                            "PDF: Form Import",
                            "PDF: Form Write",
                            "Exit",
                        }
                    )
            );

            switch (choice)
            {
                case "Excel: Beginner Import":
                    _excelBeginnerController.AddDataFromExcel();
                    break;
                case "Excel: Dynamic Import":
                    _anyExcelReadController.AddDataFromExcel();
                    break;
                case "CSV: Import":
                    csvFilePath = PromptForFilePath(csvFilePath, "CSV file");
                    _csvController.AddDataFromCsv();
                    break;
                case "PDF: Import":
                    _pdfController.AddDataFromPdf();
                    break;
                case "PDF: Form Import":
                    pdfFilePath = PromptForFilePath(pdfFilePath, "PDF form file");
                    _pdfFormController.AddOrUpdateDataFromPdfForm(pdfFilePath);
                    break;
                case "PDF: Form Write":
                    pdfFilePath = PromptForFilePath(pdfFilePath, "PDF form file");
                    var pdfFields = _pdfFormWriteController.GetExistingFieldValues(pdfFilePath);
                    _pdfFormWriteController.WriteDataToPdfForm(pdfFilePath, pdfFields);
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
