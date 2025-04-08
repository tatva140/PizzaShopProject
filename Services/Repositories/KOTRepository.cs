using DAL.Models;
using DAL.ViewModels;
using Services.Interfaces;

namespace Services.Repositories;

public class KOTRepository : IKOTRepository
{
    private readonly PizzashopContext _context;
    public KOTRepository(PizzashopContext context)
    {
        _context = context;
    }

    public OrderAppKOTViewModel GetOrders(int categoryId, string status)
    {
        List<OrderItem> orderedItems = categoryId == 0
            ? _context.OrderItems
                .Where(oi => _context.Items
                .Any(i => i.ItemId == oi.ItemId && i.IsActive))
                .ToList()
            : _context.OrderItems
                .Where(oi => _context.Items
                .Any(i => i.ItemId == oi.ItemId && i.CategoryId == categoryId && i.IsActive))
                .ToList();

        List<Order>? orders = orderedItems != null ? _context.Orders
            .ToList()
            .Where(o => orderedItems.Any(oi => oi.OrderId == o.OrderId))
            .ToList() : null;
        List<Section> sections = _context.Sections.ToList();
        OrderAppKOTViewModel orderAppKOTViewModel = new OrderAppKOTViewModel
        {
            kOTOrdersListViewModels = orders.Select(o =>
            {
                Table? table = _context.Tables.FirstOrDefault(t => t.CurrentOrderId == o.OrderId && t.IsActive == true);
                Section? section = table != null ? sections.FirstOrDefault(s => s.SectionId == table.SectionId) : null;
                List<Table> tables = _context.Tables
                    .Where(t => t.CurrentOrderId == o.OrderId && t.IsActive == true)
                    .ToList();

                return new OrderAppKOTViewModel.KOTOrdersListViewModel
                {
                    OrderId = o.OrderId,
                    SectionID = section?.SectionId ?? 0,
                    SectionName = section?.SectionName ?? string.Empty,
                    tables = tables,
                    Instruction = o.Instructions ?? "",
                    items = orderedItems
                    .Where(oi => oi.OrderId == o.OrderId &&
                                 ((status == "InProgress" && (oi.Quantity - oi.ReadyQuantity) > 0) ||
                                  (status != "InProgress" && oi.ReadyQuantity > 0)))
                        .Select(oi => new OrderAppKOTViewModel.KOTItemListViewModel
                        {
                            ItemId = oi.ItemId ?? 0,
                            ItemName = _context.Items.FirstOrDefault(i => i.ItemId == oi.ItemId)?.Name ?? string.Empty,
                            Quantity = (status == "InProgress" ? (oi.Quantity - oi.ReadyQuantity) : oi.ReadyQuantity) ?? 0,
                            Rate = oi.Rate ?? 0,
                            Status = status,
                            OrderId = o.OrderId,
                            modifiers = _context.OrderModifiers
                                .Where(mi => mi.ItemId == oi.ItemId && mi.OrderId == o.OrderId)
                                .Select(mi => new OrderAppKOTViewModel.KOTModifierListViewModel
                                {
                                    ModifierId = mi.ModifierId ?? 0,
                                    ModifierName =
                                         _context.Modifiers.FirstOrDefault(m => m.ModifierId == mi.ModifierId).ModifierName,

                                    Rate = _context.Modifiers.FirstOrDefault(m => m.ModifierId == mi.ModifierId).Rate,
                                    Quantity = _context.Modifiers.FirstOrDefault(m => m.ModifierId == mi.ModifierId).Quantity ?? 0,
                                    Instruction = mi.Instructions
                                }).ToList()
                        }).ToList(),
                    Duration = o.Duration.HasValue ? (DateTime.Now - o.Duration.Value) : TimeSpan.Zero
                };
            }).ToList()
        };
        return orderAppKOTViewModel;
    }

    public void MarkAsPrepared(List<OrderAppKOTViewModel.MarkAsPrepared> orderAppKOTViewModels)
    {
        foreach (var item in orderAppKOTViewModels)
        {
            OrderItem? orderItem = _context.OrderItems.FirstOrDefault(oi => oi.ItemId == item.ItemId && oi.OrderId == item.OrderId);
            if (orderItem != null)
            {
                if (item.Status == "InProgress")
                {
                    orderItem.ReadyQuantity = orderItem.ReadyQuantity + item.Quantity;
                }
                else
                {
                    orderItem.ReadyQuantity = orderItem.ReadyQuantity - item.Quantity;
                }
                _context.SaveChanges();
            }
        }
        var readyItems = _context.OrderItems
            .Where(oi => oi.OrderId == orderAppKOTViewModels.FirstOrDefault().OrderId && oi.ReadyQuantity == oi.Quantity)
            .ToList();
        var orderItems = _context.OrderItems
            .Where(oi => oi.OrderId == orderAppKOTViewModels.FirstOrDefault().OrderId)
            .ToList();
        Order order = _context.Orders.FirstOrDefault(o => o.OrderId == orderAppKOTViewModels.FirstOrDefault().OrderId);
        if (order != null)
        {
            if (readyItems.Count == orderItems.Count)
            {
                order.OrderStatus = "Served";
            }
            else
            {
                order.OrderStatus = "InProgress";
            }
            _context.SaveChanges();
        }

    }
}
