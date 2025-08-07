// Abstractions\ICsvTableCreator.cs
using System.Data;
using System.Threading.Tasks;

namespace ExcelReader.RyanW84.Abstractions.Data.TableCreators
{
    public interface ICsvTableCreator
    {
        Task CreateTableFromCsvDataAsync(DataTable dataTable);
        void CreateTableFromCsvData(DataTable dataTable);
    }
}