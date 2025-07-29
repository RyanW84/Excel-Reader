using System.Data;
using ExcelReader.RyanW84.Services;
using ExcelReader.RyanW84.UI;
using Spectre.Console;

namespace ExcelReader.RyanW84.Controller;

public class PdfFormController
{
    private readonly ReadFromPdfForm _readFromPdfForm;
    private readonly WriteToPdfForm _writeToPdfForm;
    private readonly WritePdfFormDataToDatabaseService _writePdfFormDataToDatabaseService;
    private readonly PdfFormWriteUI _pdfFormWriteUI;

    public PdfFormController(
        ReadFromPdfForm readFromPdfForm,
        WriteToPdfForm writeToPdfForm,
        WritePdfFormDataToDatabaseService writePdfFormDataToDatabaseService,
        PdfFormWriteUI pdfFormWriteUI)
    {
        _readFromPdfForm = readFromPdfForm;
        _writeToPdfForm = writeToPdfForm;
        _writePdfFormDataToDatabaseService = writePdfFormDataToDatabaseService;
        _pdfFormWriteUI = pdfFormWriteUI;
    }

    // You can replace this with a service or configuration lookup as needed
    private string GetDefaultPdfFilePath()
    {
        // Example default path
        return @"C:\Users\Ryanw\OneDrive\Documents\GitHub\Excel-Reader\Data\FillablePDF.pdf";
    }

    public void AddOrUpdateDataFromPdfForm(string? filePath = null)
    {
        // 1. Get the existing or default file path
        var defaultPath = filePath ?? GetDefaultPdfFilePath();

        // 2. Ask the user if they want to use the existing path or input a new one
        var useExisting = AnsiConsole.Confirm($"Use existing PDF form file path? [green]{defaultPath}[/]");
        if (!useExisting)
        {
            defaultPath = AnsiConsole.Ask<string>("Enter the path to the PDF form file:", defaultPath);
        }

        // 3. Read existing fields from PDF
        var fields = _readFromPdfForm.ReadFormFields(defaultPath);
        if (fields.Count == 0)
        {
            Console.WriteLine("No form fields found or file not found.");
            return;
        }

        // 4. Pass fields to UI for user to update
        var updatedFields = _pdfFormWriteUI.GatherUpdatedFields(fields);

        // 5. Write updated fields back to PDF form
        _writeToPdfForm.WriteFormFields(defaultPath, updatedFields);

        // 6. Add updated data to the database
        _writePdfFormDataToDatabaseService.Write(updatedFields);

        Console.WriteLine("PDF form updated and data imported to SQL table.");
    }
}
