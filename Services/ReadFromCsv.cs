﻿using System.Data;
using System.IO;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;

namespace ExcelReader.RyanW84.Services;

public class ReadFromCsv(IConfiguration configuration)
{
    private readonly IConfiguration _configuration = configuration;

    public List<string[]> ReadCsvFile()
    {
        string filePath =
            _configuration["CsvFilePath"]
            ?? @"C:\Users\Ryanw\OneDrive\Documents\GitHub\Excel-Reader\Data\ExcelCSV.csv";
        Console.WriteLine($"opening {filePath}");

        ExcelPackage.License.SetNonCommercialPersonal("Ryan Weavers");

        var csvData = new List<string[]>();

        if (!File.Exists(filePath))
        {
            Console.WriteLine("CSV file not found.");
            return csvData;
        }

        using var package = new ExcelPackage();
        using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        using var reader = new StreamReader(stream);
        var csvContent = string.Empty;

        string? line;
        while ((line = reader.ReadLine()) != null)
        {
            var rowData = line.Split(','); // Adjust for your delimiter and quoted values if needed
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
