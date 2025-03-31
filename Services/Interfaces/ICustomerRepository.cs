using DAL.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Services.Interfaces;

public interface ICustomerRepository
{
    List<CustomerListViewModel> GetCustomers(string search,string sortOrder, string time, string from, string to, int pageNumber, int pageSize, out int totalRecords);
    FileContentResult UploadExcel(string search, string time, string from, string to);
    List<CustomersViewModel> GetHistory(int id);


}
