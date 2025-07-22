using System;
using System.Collections.Generic;
using ExcelReader.RyanW84.Controller;
using ExcelReader.RyanW84.Services;

namespace ExcelReader.RyanW84.UI;

public class PdfFormWriteUI
{
    private readonly PdfFormWriteController _controller;
    private readonly ReadFromPdfForm _readFromPdfForm;

    public PdfFormWriteUI(PdfFormWriteController controller, ReadFromPdfForm readFromPdfForm)
    {
        _controller = controller;
        _readFromPdfForm = readFromPdfForm;
    }

    public void GatherInput()
    {
        Console.WriteLine("Enter the path to the PDF form file (or press Enter for default):");
        var filePath = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(filePath))
        {
            filePath = @"C:\\Users\\Ryanw\\OneDrive\\Documents\\GitHub\\Excel-Reader\\Data\\FillablePDF.pdf";
        }

        var fields = _readFromPdfForm.ReadFormFields(filePath);
        if (fields.Count == 0)
        {
            Console.WriteLine("No form fields found or file not found.");
            return;
        }

        var fieldValues = new Dictionary<string, string>();
        Console.WriteLine("Enter values for the following PDF form fields:");
        foreach (var field in fields.Keys)
        {
            Console.Write($"{field}: ");
            var value = Console.ReadLine() ?? string.Empty;
            fieldValues[field] = value;
        }

        _controller.WriteDataToPdfForm(filePath, fieldValues);
    }
}
