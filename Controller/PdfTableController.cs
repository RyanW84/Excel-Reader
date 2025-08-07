﻿using ExcelReader.RyanW84.Data;
using ExcelReader.RyanW84.Services;
using ExcelReader.RyanW84.Helpers;
using ExcelReader.RyanW84.Abstractions.Services;
using ExcelReader.RyanW84.Abstractions.FileOperations.Readers;
using ExcelReader.RyanW84.Abstractions.Data.TableCreators;
using ExcelReader.RyanW84.Abstractions.Data.DatabaseServices;

namespace ExcelReader.RyanW84.Controller;

public class PdfTableController(    
    IExcelReaderDbContext dbContext,  // ← Change this
    IPdfTableReader readFromPdf,
    ICsvTableCreator createTableFromCSV,
    INotificationService notificationService)
{
    private readonly IExcelReaderDbContext _dbContext = dbContext;
    private readonly IPdfTableReader _readFromPdf = readFromPdf;
    private readonly ICsvTableCreator _createTableFromCSV = createTableFromCSV;
    private readonly INotificationService _notificationService = notificationService;

    public async Task AddDataFromPdf()
    {
        _notificationService.ShowInfo("Starting PDF import...");
        var pdfData = await _readFromPdf.ReadPdfFileAsync();
        var dataTable = await _readFromPdf.ConvertToDataTableAsync(pdfData);
        _notificationService.ShowInfo($"Read {dataTable.Rows.Count} Rows from PDF file.");
        _notificationService.ShowInfo($"Read {dataTable.Columns.Count} Columns from PDF file.");

        dataTable.TableName = "PdfImport";
        await _createTableFromCSV.CreateTableFromCsvDataAsync(dataTable);
        await _dbContext.SaveChangesAsync();
        _notificationService.ShowSuccess("PDF import complete.");
    }
}
