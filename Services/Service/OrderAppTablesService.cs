using DAL.Models;
using DAL.ViewModels;
using Services.Interfaces;

namespace Services.Service;

public class OrderAppTablesService
{
    private readonly IOrderAppTablesRepository _orderAppTablesRepository;
    public OrderAppTablesService(IOrderAppTablesRepository orderAppTablesRepository){
        _orderAppTablesRepository=orderAppTablesRepository;
    }
    public OrderAppTablesViewModel GetTablesAndSections(List<Section> sections){
        return _orderAppTablesRepository.GetTablesAndSections(sections);
    }
}
