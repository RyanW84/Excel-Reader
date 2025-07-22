using iText.Forms;
using iText.Forms.Fields;
using iText.Kernel.Pdf;

namespace ExcelReader.RyanW84.Services;

public class WriteToPdfForm
{
    public void WriteFormFields(string filePath, Dictionary<string, string> fieldValues)
    {
        if (!File.Exists(filePath))
        {
            Console.WriteLine($"File not found: {filePath}");
            return;
        }

        // Open for modification
        using var pdfReader = new PdfReader(filePath);
        using var pdfWriter = new PdfWriter(filePath + ".tmp");
        using var pdfDoc = new PdfDocument(pdfReader, pdfWriter);
        var form = PdfAcroForm.GetAcroForm(pdfDoc, true);
        if (form == null)
        {
            Console.WriteLine("No AcroForm found in PDF.");
            return;
        }
        var fields = form.GetAllFormFields();
        foreach (var kvp in fieldValues)
        {
            if (fields.ContainsKey(kvp.Key))
            {
                var field = fields[kvp.Key];
                // Special handling for the "wanted" checkbox
                if (kvp.Key.Equals("wanted", StringComparison.OrdinalIgnoreCase) && field is PdfButtonFormField)
                {
                    if (kvp.Value.Equals("Yes", StringComparison.OrdinalIgnoreCase))
                        ((PdfButtonFormField)field).SetValue("Yes");
                    else
                        ((PdfButtonFormField)field).SetValue("Off");
                }
                else
                {
                    field.SetValue(kvp.Value);
                }
            }
        }
        //form.FlattenFields();
        pdfDoc.Close();
        pdfReader.Close();
        pdfWriter.Close();
        // Replace original file
        File.Delete(filePath);
        File.Move(filePath + ".tmp", filePath);
    }
}
