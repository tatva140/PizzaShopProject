using DAL.Models;
using DAL.ViewModels;

namespace Services.Interfaces;

public interface IOrderAppTablesRepository
{
    OrderAppTablesViewModel GetTablesAndSections(List<Section> sections);
    WaitingTokenListViewModel WaitingTokenList(int id);
    OrderAppCustomerViewModel CustomerDetails(string email);
    OrderAppCustomerViewModel WaitingTokenCustomerDetails(int id);
    CustomErrorViewModel AssignTable(OrderAppCustomerViewModel orderAppCustomerViewModel);
    CustomErrorViewModel AddWaitingToken(OrderAppCustomerViewModel orderAppCustomerViewModel);
}
