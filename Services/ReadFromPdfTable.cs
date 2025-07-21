using System.Data;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using Microsoft.Extensions.Configuration;

namespace ExcelReader.RyanW84.Services;

public class ReadFromPdf(IConfiguration configuration)
{
    private readonly IConfiguration _configuration = configuration;

    public List<string[]> ReadPdfFile()
    {
        string filePath =
            _configuration["PdfFilePath"]
            ?? @"C:\Users\Ryanw\OneDrive\Documents\GitHub\Excel-Reader\Data\TablePDF.pdf";
        Console.WriteLine($"opening {filePath}");

        var pdfData = new List<string[]>();

        if (!File.Exists(filePath))
        {
            Console.WriteLine("PDF file not found.");
            return pdfData;
        }

        using var pdfReader = new PdfReader(filePath);
        using var pdfDoc = new PdfDocument(pdfReader);

        for (int page = 1; page <= pdfDoc.GetNumberOfPages(); page++)
        {
            var strategy = new SimpleTextExtractionStrategy();
            var text = PdfTextExtractor.GetTextFromPage(pdfDoc.GetPage(page), strategy);
            var lines = text.Split('\n');
            foreach (var line in lines)
            {
                var rowData = line.Split(','); // Adjust for your delimiter if needed
                pdfData.Add(rowData);
            }
        }

        return pdfData;
    }

    public DataTable ConvertToDataTable(List<string[]> pdfData)
    {
        var dataTable = new DataTable();

        if (pdfData == null || pdfData.Count < 2)
            return dataTable;

        // Assume first row is the title, second row is the column headers
        // Join the header row into a single string, then split by '_'
        var headerRow = string.Join(",", pdfData[2]);
        var columnHeaders = headerRow.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        foreach (var columnName in columnHeaders)
        {
            dataTable.Columns.Add(columnName.Trim());
        }

        for (int i = 3; i < pdfData.Count; i++)
        {
            var dataRow = string.Join(",", pdfData[i]);
            var readRow = dataRow.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            dataTable.Rows.Add(readRow);
        }
        return dataTable;
    }
}
