using DAL.Models;
using DAL.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Services.Interfaces;

public interface IOrderRepository
{
       List<OrdersViewModel> GetOrders(string search,string status,string time,string from,string to, int pageNumber, int pageSize, out int totalRecords);
       FileContentResult UploadExcel(string search,string status,string time,string from,string to);

}
