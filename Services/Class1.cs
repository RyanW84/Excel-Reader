using System;

namespace ExcelReader.RyanW84.Services;

public class ConvertDictionaryToDataTable
	{
	// This class converts a Dictionary<string, object> to a DataTable.
	// It creates a DataTable with columns based on the dictionary keys
	// and adds a single row with the corresponding values.
	// If a value is null, it will be replaced with DBNull.Value.

	public class ConvertDictionaryToDataTable
		{
		public DataTable Convert(Dictionary<string , object> dictionary)
			{
			var dataTable = new DataTable();
			foreach(var key in dictionary.Keys)
				{
				dataTable.Columns.Add(key);
				}
			var row = dataTable.NewRow();
			foreach(var kvp in dictionary)
				{
				row[kvp.Key] = kvp.Value ?? DBNull.Value;
				}
			dataTable.Rows.Add(row);
			return dataTable;
			}
		}
	}