using DAL.Models;
using Services.Interfaces;

namespace Services.Repositories;

public class TaxAndFeesRepository:ITaxAndFeesRepository
{
    private readonly PizzashopContext _context;

    public TaxAndFeesRepository(PizzashopContext context){
        _context=context;
    }
     public List<Tax> GetTaxes( string search, int pageNumber, int pageSize, out int totalRecords)
    {
       
        if (search != null)
        {
            IQueryable<Tax> taxes1 = _context.Taxes.Where(m => m.IsActive == true  && m.TaxName.ToLower().Contains(search.ToLower()));
            totalRecords = taxes1.Count();
            return taxes1.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        }
        IQueryable<Tax> taxes = _context.Taxes.Where(m => m.IsActive == true );
        totalRecords = taxes.Count();
        return taxes.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
    }
}
