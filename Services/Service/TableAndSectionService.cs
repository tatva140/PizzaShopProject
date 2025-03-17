using System.Text.Json.Nodes;
using DAL.Models;
using DAL.ViewModels;
using Services.Interfaces;

namespace Services.Service;

public class TableAndSectionService
{
      private readonly ITableAndSectionRepository _tableAndSectionRepository;

    public TableAndSectionService(ITableAndSectionRepository tableAndSectionRepository)
    {
        _tableAndSectionRepository = tableAndSectionRepository;
    }
    public List<Section> GetSections(){
        return _tableAndSectionRepository.GetSections();
    }

     public (List<Table>,int totalRecords) GetTables(int id,string search,int pageNumber,int pageSize){
        int totalRecords;
        List<Table> tables=_tableAndSectionRepository.GetTables(id,search,pageNumber,pageSize,out totalRecords);
        return  (tables,totalRecords);
     }
      public int AddSection(Section section){
        return _tableAndSectionRepository.AddSection(section);
    }
     public bool DeleteSection(int id){
        return _tableAndSectionRepository.DeleteSection(id);
    }
       public Section GetSectionDetails( int id){
       return _tableAndSectionRepository.GetSectionDetails(id);
    }
    public int EditSection(Section section){
        return _tableAndSectionRepository.EditSection(section);
    }
     public int AddTable(Table table){
        return _tableAndSectionRepository.AddTable(table);
    }
    public Table GetTableDetails( int id){
       return _tableAndSectionRepository.GetTableDetails(id);
    }
     public int EditTable(Table table){
        return _tableAndSectionRepository.EditTable(table);
    }
      public bool DeleteTables(JsonArray ids){
        return _tableAndSectionRepository.DeleteTables(ids);
    }
     public int DeleteTable(int id){
        return _tableAndSectionRepository.DeleteTable(id);
    }
}
