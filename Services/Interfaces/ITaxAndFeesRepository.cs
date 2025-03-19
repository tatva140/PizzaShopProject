using DAL.Models;
using DAL.ViewModels;

namespace Services.Interfaces;

public interface ITaxAndFeesRepository
{
    List<Tax> GetTaxes(string search, int pageNumber, int pageSize, out int totalRecords);
    int AddTax(Tax tax);
    int DeleteTax(int id);
    int EditTax(TaxAndFeesViewModel taxAndFeesViewModel);
    Tax GetTaxDetails(int id);

}
