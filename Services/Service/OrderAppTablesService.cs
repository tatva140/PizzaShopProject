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
    public WaitingTokenListViewModel WaitingTokenList(int id){
        return _orderAppTablesRepository.WaitingTokenList(id);
    }
    public OrderAppCustomerViewModel CustomerDetails(string email){
        return _orderAppTablesRepository.CustomerDetails(email);
    }
    public OrderAppCustomerViewModel WaitingTokenCustomerDetails(int id){
        return _orderAppTablesRepository.WaitingTokenCustomerDetails(id);
    }
    public CustomErrorViewModel AssignTable(OrderAppCustomerViewModel orderAppCustomerViewModel){
        return _orderAppTablesRepository.AssignTable(orderAppCustomerViewModel);
    }
    public CustomErrorViewModel AddWaitingToken(OrderAppCustomerViewModel orderAppCustomerViewModel){
        return _orderAppTablesRepository.AddWaitingToken(orderAppCustomerViewModel);
    }
}
