using DAL.Models;
using DAL.ViewModels;

namespace Services.Interfaces;

public interface IOrderAppTablesRepository
{
    OrderAppTablesViewModel GetTablesAndSections(List<Section> sections);
}
