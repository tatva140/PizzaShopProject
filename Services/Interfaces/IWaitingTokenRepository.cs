using DAL.Models;
using DAL.ViewModels;

namespace Services.Interfaces;

public interface IWaitingTokenRepository
{
    WaitingToken GetWaitingToken(int id);

    void Update(WaitingToken waitingToken);
     List<Table> AvailableTables(int id);
     CustomErrorViewModel AssignTable(OrderAppCustomerViewModel orderAppCustomerViewModel);
}
