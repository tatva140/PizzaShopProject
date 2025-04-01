namespace DAL.ViewModels;

public class AssignCustomerTablesViewModel
{
    public string email {get;set;}
    public string name {get;set;}
    public string phone {get;set;}
    public int noOfPersons {get;set;}
    public int sectionId {get;set;}
    public int waitingTokenId {get;set;}
    public List<int> selectedTables {get;set;}
}
