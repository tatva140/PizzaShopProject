using DAL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace Services.Service;

public class CustomerService
{
    private readonly ICustomerRepository _customerRepository;
    public CustomerService(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }
    public (List<CustomerListViewModel>, int totalRecords) GetCustomers(string search, string time, string from, string to, int pageNumber, int pageSize)
    {
        int totalRecords;
        List<CustomerListViewModel> customers = _customerRepository.GetCustomers(search, time, from, to, pageNumber, pageSize, out totalRecords);
        return (customers, totalRecords);
    }
    public FileContentResult UploadExcel(string search, string time, string from, string to)
    {
        return _customerRepository.UploadExcel(search, time, from, to);
    }

    public List<CustomersViewModel> GetHistory(int id){
        return _customerRepository.GetHistory(id);
    }
}
