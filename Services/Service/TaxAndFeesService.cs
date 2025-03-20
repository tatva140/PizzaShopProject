using DAL.Models;
using DAL.ViewModels;
using Services.Interfaces;

namespace Services.Service;

public class TaxAndFeesService
{
  private readonly ITaxAndFeesRepository _taxAndFeesRepository;
  public TaxAndFeesService(ITaxAndFeesRepository taxAndFeesRepository)
  {
    _taxAndFeesRepository = taxAndFeesRepository;
  }

  public (List<Tax>, int totalRecords) GetTaxes(string search, int pageNumber, int pageSize)
  {
    int totalRecords;
    List<Tax> taxes = _taxAndFeesRepository.GetTaxes(search, pageNumber, pageSize, out totalRecords);
    return (taxes, totalRecords);
  }
  public int AddTax(Tax tax)
  {
    return _taxAndFeesRepository.AddTax(tax);
  }
  public CustomErrorViewModel DeleteTax(int id)
  {
    return _taxAndFeesRepository.DeleteTax(id);
  }
  public CustomErrorViewModel EditTax(TaxAndFeesViewModel taxAndFeesViewModel)
  {
    return _taxAndFeesRepository.EditTax(taxAndFeesViewModel);
  }
  public Tax GetTaxDetails(int id)
  {
    return _taxAndFeesRepository.GetTaxDetails(id);
  }
}
