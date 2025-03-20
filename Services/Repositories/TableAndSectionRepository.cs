using System.Text.Json.Nodes;
using DAL.Models;
using DAL.ViewModels;
using Services.Interfaces;

namespace Services.Repositories;

public class TableAndSectionRepository : ITableAndSectionRepository
{
    private readonly PizzashopContext _context;

    public TableAndSectionRepository(PizzashopContext context)
    {
        _context = context;
    }
    public List<Section> GetSections()
    {
        return _context.Sections.Where(c => c.IsActive == true).ToList();
    }
    public List<Table> GetTables(int id, string search, int pageNumber, int pageSize, out int totalRecords)
    {
        if (id == 0)
        {
            if (search != null)
            {
                IQueryable<Table> tables1 = _context.Tables.Where(m => m.IsActive == true && m.TableName.ToLower().Contains(search.ToLower()));
                totalRecords = tables1.Count();
                return tables1.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            }
            IQueryable<Table> tables = _context.Tables.Where(m => m.IsActive == true);
            totalRecords = tables.Count();
            return tables.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        }
        if (search != null)
        {
            IQueryable<Table> tables1 = _context.Tables.Where(m => m.IsActive == true && m.SectionId == id && m.TableName.ToLower().Contains(search.ToLower()));
            totalRecords = tables1.Count();
            return tables1.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        }
        IQueryable<Table> table = _context.Tables.Where(m => m.IsActive == true && m.SectionId == id);

        totalRecords = table.Count();
        return table.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
    }
    public int AddSection(Section section)
    {

        if (section.SectionName == null) return 0;

        Section section1 = new Section
        {
            SectionName = section.SectionName,
            Description = section.Description ?? ""
        };
        _context.Sections.Add(section1);
        _context.SaveChanges();


        return section1.SectionId;
    }
    public bool DeleteSection(int id)
    {
        Section section = _context.Sections.FirstOrDefault(u => u.SectionId == id && u.IsActive == true);
        if (section == null) return false;

        List<Table> tables = _context.Tables.Where(c => c.SectionId == id).ToList();
        foreach (var table in tables)
        {
            if(table.TableStatus=="Occupied")return false;
            table.IsActive = false;
        }

        section.IsActive = false;
        _context.SaveChanges();
        return true;
    }

    public Section GetSectionDetails(int id)
    {
        return _context.Sections.Find(id);
    }
    public int EditSection(Section section)
    {
        Section section1 = _context.Sections.FirstOrDefault(mg => mg.SectionId == section.SectionId);
        if (section1 == null) return 0;
        section1.SectionName = section.SectionName;
        section1.Description = section.Description ?? section.Description;


        _context.SaveChanges();
        return section1.SectionId;
    }
    public int AddTable(Table table)
    {
        if (table.TableName == null || table.SectionId == null || table.Capacity == null || table.TableStatus == null)
        {
            return 0;
        }
        _context.Tables.Add(table);
        _context.SaveChanges();

        return table.SectionId ?? 0;
    }
    public Table GetTableDetails(int id)
    {
        return _context.Tables.Find(id);
    }
    public int EditTable(TablesAndSectionViewModel tablesAndSectionViewModel)
    {
        Table table1 = _context.Tables.FirstOrDefault(mg => mg.TableId == tablesAndSectionViewModel.TableId);
        if (table1 == null) return 0;
        table1.TableName = tablesAndSectionViewModel.TableName??table1.TableName;
        table1.Capacity = tablesAndSectionViewModel.Capacity??table1.Capacity;
        table1.TableStatus = tablesAndSectionViewModel.TableStatus??table1.TableStatus;
        table1.SectionId = tablesAndSectionViewModel.SectionId;


        _context.SaveChanges();
        return table1.SectionId ?? 0;
    }
    public bool DeleteTables(JsonArray ids)
    {
        if (ids.Count == 0) return false;
        foreach (int tableId in ids)
        {
            Table table = _context.Tables.FirstOrDefault(i => i.TableId == tableId && i.IsActive == true);
            if (table == null || table.TableStatus=="Occupied") return false;
            table.IsActive = false;
        }
        _context.SaveChanges();
        return true;
    }
    public int DeleteTable(int id)
    {
        Table table = _context.Tables.FirstOrDefault(u => u.TableId == id && u.IsActive == true);
        if (table == null || table.TableStatus=="Occupied") return 0;
        table.IsActive = false;

        _context.SaveChanges();
        return table.SectionId ?? 0;
    }

}
