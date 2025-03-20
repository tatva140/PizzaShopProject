using DAL.Models;
using DAL.ViewModels;
using Services.Interfaces;

namespace Services.Repositories;

public class TaxAndFeesRepository : ITaxAndFeesRepository
{
    private readonly PizzashopContext _context;

    public TaxAndFeesRepository(PizzashopContext context)
    {
        _context = context;
    }
    public List<Tax> GetTaxes(string search, int pageNumber, int pageSize, out int totalRecords)
    {

        if (search != null)
        {
            IQueryable<Tax> taxes1 = _context.Taxes.Where(m => m.IsActive == true && m.TaxName.ToLower().Contains(search.ToLower()));
            totalRecords = taxes1.Count();
            return taxes1.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        }
        IQueryable<Tax> taxes = _context.Taxes.Where(m => m.IsActive == true);
        totalRecords = taxes.Count();
        return taxes.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
    }
    public int AddTax(Tax tax)
    {
        _context.Taxes.Add(tax);
        _context.SaveChanges();

        return 1;
    }
    public CustomErrorViewModel DeleteTax(int id)
    {
        Tax tax = _context.Taxes.FirstOrDefault(u => u.TaxId == id && u.IsActive == true);
        if (tax == null) return new CustomErrorViewModel { Status = false, Message = "Tax not found!" };
        tax.IsActive = false;

        _context.SaveChanges();
        return new CustomErrorViewModel { Status = true, Message = "Tax Deleted Successfully!" };

    }
    public CustomErrorViewModel EditTax(TaxAndFeesViewModel taxAndFeesViewModel)
    {
        Tax tax = _context.Taxes.FirstOrDefault(mg => mg.TaxId == taxAndFeesViewModel.TaxId);
        if (tax == null) return new CustomErrorViewModel { Status = false, Message = "Tax not found!" };
        tax.TaxName = taxAndFeesViewModel.TaxName;
        tax.Amount = taxAndFeesViewModel.Amount;
        tax.IsEnabled = taxAndFeesViewModel.IsEnabled;
        tax.Type = taxAndFeesViewModel.Type;

        _context.SaveChanges();
        return new CustomErrorViewModel { Status = true, Message = "Tax Edited Successfully!" };
    }
    public Tax GetTaxDetails(int id)
    {
        return _context.Taxes.Find(id);
    }
}
