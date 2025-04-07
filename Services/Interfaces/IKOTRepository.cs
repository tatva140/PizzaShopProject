using DAL.ViewModels;

namespace Services.Interfaces;

public interface IKOTRepository
{
    OrderAppKOTViewModel GetOrders(int categoryId,string status);
    void MarkAsPrepared(List<OrderAppKOTViewModel.MarkAsPrepared> orderAppKOTViewModels);
}
