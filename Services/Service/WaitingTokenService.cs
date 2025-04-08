using DAL.Models;
using DAL.ViewModels;
using Services.Interfaces;

namespace Services.Service;

public class WaitingTokenService
{
    private readonly IWaitingTokenRepository _waitingTokenRepository;
    public WaitingTokenService(IWaitingTokenRepository waitingTokenRepository)
    {
        _waitingTokenRepository = waitingTokenRepository;
    }
    // public OrderAppTablesViewModel GetSections()
    // {
    //     OrderAppTablesViewModel orderAppTablesViewModel = new OrderAppTablesViewModel();
    //     // orderAppTablesViewModel.Sections = sections;
    //     var sections = _waitingTokenRepository.GetSections();
    //     return orderAppTablesViewModel;
    // }
}
