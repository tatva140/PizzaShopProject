using DAL.Models;
using DAL.ViewModels;
using Services.Interfaces;

namespace Services.Service;

public class WaitingTokenService
{
    private readonly IWaitingTokenRepository _waitingTokenRepository;
    private readonly ITableAndSectionRepository _tableAndSectionRepository;
    public WaitingTokenService(IWaitingTokenRepository waitingTokenRepository, ITableAndSectionRepository tableAndSectionRepository)

    {
        _waitingTokenRepository = waitingTokenRepository;
        _tableAndSectionRepository = tableAndSectionRepository;

    }
    public OrderAppTablesViewModel GetSections()
    {
        OrderAppTablesViewModel orderAppTablesViewModel = new OrderAppTablesViewModel();
        var sections = _tableAndSectionRepository.GetSections();
        orderAppTablesViewModel.Sections = new List<OrderAppTablesViewModel.SectionListViewModel>();
        foreach (var section in sections)
        {
            OrderAppTablesViewModel.SectionListViewModel sectionListViewModel = new OrderAppTablesViewModel.SectionListViewModel
            {
                SectionName = section.SectionName,
                SectionId = section.SectionId,
            };

            orderAppTablesViewModel.Sections.Add(sectionListViewModel);
        }
        return orderAppTablesViewModel;
    }

    public CustomErrorViewModel DeleteWaitingToken(int id)
    {
        WaitingToken waitingToken = _waitingTokenRepository.GetWaitingToken(id);
        if (waitingToken == null)
        {
            return new CustomErrorViewModel { Message = "No waiting Token found", Status = false };
        }
        waitingToken.IsActive = false;
        _waitingTokenRepository.Update(waitingToken);
        return new CustomErrorViewModel { Message = "Waiting Token Deleted Successfully!", Status = true };
    }

    public List<Table> AvailableTables(int id){
        List<Table> tables=_waitingTokenRepository.AvailableTables(id);
        return tables;
    }

    public CustomErrorViewModel AssignTable(OrderAppCustomerViewModel orderAppCustomerViewModel)
    {
        return _waitingTokenRepository.AssignTable(orderAppCustomerViewModel);
    }
}
