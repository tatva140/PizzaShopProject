
using System.Text.Json.Nodes;
using DAL.Models;
using DAL.ViewModels;

namespace Services.Interfaces;

public interface ITableAndSectionRepository
{
    List<Section> GetSections();
    List<Table> GetTables(int id,string search, int pageNumber, int pageSize, out int totalRecords);
    int AddSection(Section section);
    bool DeleteSection(int id);
    Section GetSectionDetails( int id);
    int EditSection(Section section);
    int AddTable(Table table);
    Table GetTableDetails( int id);
     int EditTable(Table table);
     bool DeleteTables(JsonArray ids);
     int DeleteTable(int id);

}
