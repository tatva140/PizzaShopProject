using DAL.Models;
using Services.Interfaces;

namespace Services.Repositories;

public class WaitingTokenRepository:IWaitingTokenRepository
{
    private readonly PizzashopContext _context;
    public WaitingTokenRepository(PizzashopContext context)
    {
        _context = context;
    }
    // public OrderAppTablesViewModel GetSections()
    // {
        
    //     return orderAppTablesViewModel;
    // }
}
