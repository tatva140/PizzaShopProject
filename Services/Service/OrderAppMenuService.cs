using DAL.Models;
using DAL.ViewModels;
using Services.Interfaces;

namespace Services.Service;

public class OrderAppMenuService
{
    private readonly IOrderAppMenuRepository _orderAppMenuRepository;
    private readonly IMenuRepository _menuRepository;
    public OrderAppMenuService(IOrderAppMenuRepository orderAppMenuRepository,IMenuRepository menuRepository){
        _orderAppMenuRepository=orderAppMenuRepository;
        _menuRepository=menuRepository;
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
}
