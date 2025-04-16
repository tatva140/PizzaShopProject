using DAL.Models;
using DAL.ViewModels;
using Services.Interfaces;

namespace Services.Service;

public class OrderAppMenuService
{
    private readonly IOrderAppMenuRepository _orderAppMenuRepository;
    private readonly IMenuRepository _menuRepository;
    private readonly IOrderRepository _orderRepository;
    public OrderAppMenuService(IOrderAppMenuRepository orderAppMenuRepository,IMenuRepository menuRepository,IOrderRepository orderRepository){
        _orderAppMenuRepository=orderAppMenuRepository;
        _menuRepository=menuRepository;
        _orderRepository=orderRepository;
    }

    public OrderAppMenuViewModel GetCategories(){
        List<Category> categories=_menuRepository.GetCategories();
        OrderAppMenuViewModel orderAppMenuViewModel=new OrderAppMenuViewModel{
            categories=categories
        };
        return orderAppMenuViewModel;
    }

    public OrderAppMenuViewModel GetItems(int id){
        List<Item> items=_orderAppMenuRepository.GetCategoryItems(id);
        OrderAppMenuViewModel orderAppMenuViewModel=new OrderAppMenuViewModel{
            items=items
        };
        return orderAppMenuViewModel;
    }

    public void FavouriteItem(int id,bool flag){
        Item item=_orderAppMenuRepository.GetItem(id);
        item.Isfavourite=flag;
        _orderAppMenuRepository.Update(item);
    }

    public OrderAppMenuViewModel GetModifierDetails(int id){
        OrderAppMenuViewModel orderAppMenuViewModel=_orderAppMenuRepository.GetModifierDetails(id);
        return orderAppMenuViewModel;
    }

    public OrderAppMenuViewModel GetOrderPreview(int id)
    {
        List<Table> tables=_orderAppMenuRepository.GetCustomerTables(id);
        Section section=_orderAppMenuRepository.GetSection(tables[0].SectionId??0);
        OrdersListViewModel ordersListViewModel=new OrdersListViewModel();
        if(tables[0].CurrentOrderId!=null){
             ordersListViewModel=_orderRepository.GetOrderDetails(tables[0].CurrentOrderId??0);
        }else{
            ordersListViewModel=null;
        }
        List<Tax> taxes=_orderAppMenuRepository.GetTax();
        OrderAppMenuViewModel orderAppMenuViewModel=new OrderAppMenuViewModel{
            tables=tables,
            SectionName=section.SectionName,
            ordersListViewModel=ordersListViewModel,
            taxes=taxes
        };
        return orderAppMenuViewModel;
    }

    // public void AddOrderItem(OrderAppMenuViewModel orderAppMenuViewModel){

    // }
}
