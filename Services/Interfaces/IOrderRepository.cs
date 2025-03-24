using DAL.Models;
using DAL.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Services.Interfaces;

public interface IOrderRepository
{
       List<OrdersListViewModel> GetOrders(string search,string status,string time,string from,string to, int pageNumber, int pageSize, out int totalRecords);
       FileContentResult UploadExcel(string search,string status,string time,string from,string to);
       OrdersListViewModel GetOrderDetails(int id);

}
