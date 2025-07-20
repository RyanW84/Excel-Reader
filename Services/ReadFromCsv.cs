using OfficeOpenXml;

using System.Data;

namespace ExcelReader.RyanW84.Services;
public class ReadFromCsv
{
	public List<string[]> ReadCsvFile( )
	{
		string filePath = @"C:\Users\Ryanw\OneDrive\Documents\GitHub\Excel-Reader\Data\ExcelCSV.xlsx";
		var excelPackage = new ExcelPackage(new FileInfo(filePath));
		var worksheet = excelPackage.Workbook.Worksheets.First();
		var rowCount = worksheet.Dimension.Rows;
		var colCount = worksheet.Dimension.Columns;
		var csvData = new List<string[]>();

		for (var row = 1; row <= rowCount; row++)
		{
			var rowData = new string[colCount];

			for (var col = 1; col <= colCount; col++)
			{
				rowData[col - 1] = worksheet.Cells[row , col].Text;
			}

			csvData.Add(rowData);
		}

		return csvData;
	}

	public DataTable ConvertToDataTable(List<string[]> csvData)
	{
		var dataTable = new DataTable();

		if (csvData == null || csvData.Count == 0)
			return dataTable;

		// Add columns using the first row as header
		foreach (var columnName in csvData[0])
		{
			dataTable.Columns.Add(columnName ?? string.Empty);
		}

		// Add rows (skip header row)
		for (int i = 1; i < csvData.Count; i++)
		{
			dataTable.Rows.Add(csvData[i]);
		}

		return dataTable;
	}
}
