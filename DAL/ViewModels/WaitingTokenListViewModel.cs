using DAL.Models;

namespace DAL.ViewModels;

public class WaitingTokenListViewModel
{
    public int SectionId {get;set;}
    public List<WaitingToken> waitingTokens {get;set;}

}
