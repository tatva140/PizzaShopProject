using DAL.Models;
using DAL.ViewModels;
using Services.Interfaces;

namespace Services.Repositories;

public class KOTRepository:IKOTRepository
{
    private readonly PizzashopContext _context;
    public KOTRepository(PizzashopContext context)
    {
        _context = context;
    }

    // public List<OrderAppKOTViewModel> GetOrders(int categoryId)
    // {
    //     var orderedItems=_context.OrderItems.ToList();
    //     foreach(var item in orderedItems)
    //     {
    //         var items=_context.Items.Where(i=>i.ItemId==item.ItemId && i.CategoryId==categoryId && i.IsActive==true).ToList();
    //         if(items.Count>0)
    //         {
    //         var orders = _context.Orders
    //             .Where(o => items.Any(i => i.OrderId == o.OrderId))
    //             .ToList();

    //         return orders.Select(o => new OrderAppKOTViewModel
    //         {
    //             OrderId = o.OrderId,
    //             OrderDate = o.OrderDate,
    //             CustomerName = o.CustomerName,
    //             TotalAmount = o.TotalAmount
    //         }).ToList();
    //         }
    //     }
      
    // }
}
