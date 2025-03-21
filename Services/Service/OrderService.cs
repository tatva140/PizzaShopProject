using DAL.Models;
using DAL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace Services.Service;

public class OrderService
{
    private readonly IOrderRepository _orderRepository;

    public OrderService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public (List<OrdersViewModel>, int totalRecords) GetOrders(string search, string status, string time, string from, string to, int pageNumber, int pageSize)
    {
        int totalRecords;
        List<OrdersViewModel> orders = _orderRepository.GetOrders(search, status, time, from, to, pageNumber, pageSize, out totalRecords);
        return (orders, totalRecords);
    }

    public FileContentResult UploadExcel(string search,string status,string time,string from,string to){
        return _orderRepository.UploadExcel(search,status,time,from,to);
    }


}
