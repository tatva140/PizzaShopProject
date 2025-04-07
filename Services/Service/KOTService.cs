using DAL.ViewModels;
using Services.Interfaces;

namespace Services.Service;

public class KOTService
{
    private readonly IKOTRepository _kotRepository;
    public KOTService(IKOTRepository kotRepository)
    {
        _kotRepository = kotRepository;
    }
    public OrderAppKOTViewModel GetOrders(int categoryId,string status)
    {
        return _kotRepository.GetOrders(categoryId,status);
    }
    public void MarkAsPrepared(List<OrderAppKOTViewModel.MarkAsPrepared> orderAppKOTViewModels)
    {
        _kotRepository.MarkAsPrepared(orderAppKOTViewModels);
    }
    
}
