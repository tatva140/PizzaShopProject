using DAL.Models;
using DAL.ViewModels;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using DAL.ViewModels; // Ensure this namespace contains SectionViewModel
using Services.Interfaces;
using static DAL.ViewModels.OrderAppTablesViewModel;

namespace Services.Repositories;

public class OrderAppTablesRepository : IOrderAppTablesRepository
{
    private readonly PizzashopContext _context;

    public OrderAppTablesRepository(PizzashopContext context)
    {
        _context = context;
    }

    public OrderAppTablesViewModel GetTablesAndSections(List<Section> sections)
    {
        OrderAppTablesViewModel orderAppTablesViewModel = new OrderAppTablesViewModel();

        
        orderAppTablesViewModel.Sections = (from s in sections

                            select new SectionListViewModel
                            {
                            SectionName = s.SectionName,
                            SectionId = s.SectionId,
                            Tables = (from se in sections
                                  join t in _context.Tables on se.SectionId equals t.SectionId
                                  
                                  where t.IsActive == true && t.SectionId == s.SectionId
                                  select new OrderAppTableListViewModel
                                  {
                                      TableName = t.TableName,
                                      Capacity = t.Capacity ?? 0,
                                    //   Duration = o != null && o.Duration.HasValue ? (DateTime.Now - o.Duration.Value) : (DateTime.Now-TimeSpan.Zero),
                                    //   TotalAmount = o?.TotalAmount ?? 0
                                  }).ToList(),
                            }).ToList();






        return orderAppTablesViewModel;
    }
}
