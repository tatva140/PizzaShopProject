using DAL.Models;

namespace Services.Interfaces;

public interface ITaxAndFeesRepository
{
    List<Tax> GetTaxes(string search, int pageNumber, int pageSize, out int totalRecords);

}
