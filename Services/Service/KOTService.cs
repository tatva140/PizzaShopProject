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
    // public List<OrderAppKOTViewModel> GetOrders(int categoryId)
    // {
    //     return _kotRepository.GetOrders(categoryId);
    // }
    
}
