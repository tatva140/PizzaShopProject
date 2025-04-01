using DAL.Models;
using DAL.ViewModels;

namespace Services.Interfaces;

public interface IOrderAppTablesRepository
{
    OrderAppTablesViewModel GetTablesAndSections(List<Section> sections);
    WaitingTokenListViewModel WaitingTokenList();
    OrderAppCustomerViewModel CustomerDetails(string email);
    OrderAppCustomerViewModel WaitingTokenCustomerDetails(string email);
    CustomErrorViewModel AssignTable(AssignCustomerTablesViewModel assignCustomerTablesViewModel);
}
