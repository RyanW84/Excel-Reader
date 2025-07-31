namespace ExcelReader.RyanW84.Helpers;

public static class ExcelFieldExtractor
{
    public static Dictionary<string, string> ExtractFields(System.Data.DataTable table)
    {
        var fields = new Dictionary<string, string>();
        if (table == null || table.Rows.Count == 0)
            return fields;

        foreach (System.Data.DataColumn col in table.Columns)
        {
            fields[col.ColumnName] = table.Rows[0][col.ColumnName]?.ToString() ?? string.Empty;
        }
        return fields;
    }
}