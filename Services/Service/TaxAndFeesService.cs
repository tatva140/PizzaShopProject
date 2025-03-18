using DAL.Models;
using Services.Interfaces;

namespace Services.Service;

public class TaxAndFeesService
{
    private readonly ITaxAndFeesRepository _taxAndFeesRepository;
    public TaxAndFeesService(ITaxAndFeesRepository taxAndFeesRepository){
        _taxAndFeesRepository=taxAndFeesRepository;
    }

      public (List<Tax>,int totalRecords) GetTaxes(string search,int pageNumber,int pageSize){
        int totalRecords;
        List<Tax> taxes=_taxAndFeesRepository.GetTaxes(search,pageNumber,pageSize,out totalRecords);
        return  (taxes,totalRecords);
     }
}
