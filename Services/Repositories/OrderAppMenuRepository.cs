using DAL.Models;
using Microsoft.AspNetCore.Http.Internal;
using Services.Interfaces;

namespace Services.Repositories;

public class OrderAppMenuRepository:IOrderAppMenuRepository
{
    private readonly PizzashopContext _context;
    public OrderAppMenuRepository(PizzashopContext context){
        _context=context;
    }
    public List<Item> GetCategoryItems(int id){
        List<Item> items = new List<Item>();
        if(id==-1){
         items=_context.Items.Where(i=>i.Isfavourite==true && i.IsActive==true).ToList();
        }else if(id==0){

         items=_context.Items.Where(i=> i.IsActive==true).ToList();
        }else{

         items=_context.Items.Where(i=>i.CategoryId==id && i.IsActive==true).ToList();
        }
        return items;
    }
    public Item GetItem(int id){
        Item item=_context.Items.Where(i=>i.ItemId==id).FirstOrDefault();
        return item;
    }
    public void Update(Item item){
        _context.Items.Update(item);
        _context.SaveChanges();
    }
}
