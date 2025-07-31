using Spectre.Console;

namespace ExcelReader.RyanW84.Helpers;

public class PdfFilePathSelector
{
    private readonly string _defaultPath = @"C:\Users\Ryanw\OneDrive\Documents\GitHub\Excel-Reader\Data\FillablePDF.pdf";

    public string GetPdfFilePath(string? filePath = null)
    {
        var path = filePath ?? _defaultPath;
        var useExisting = AnsiConsole.Confirm($"Use existing PDF form file path? [green]{path}[/]");
        if (!useExisting)
        {
            path = AnsiConsole.Ask("Enter the path to the PDF form file:", path);
        }
        return path;
    }
}