namespace DAL.ViewModels;

public class OrderAppTablesViewModel
{

        public List<SectionListViewModel> Sections {get;set;}
 

public class OrderAppTableListViewModel{
 public string TableName {get;set;}

    public int Capacity {get;set;}

    public decimal TotalAmount {get;set;}

    public DateTime Duration {get;set;}
}
public class SectionListViewModel{
    public string SectionName {get;set;}
    public int SectionId {get;set;}
    public List<OrderAppTableListViewModel> Tables {get;set;}

}
  

}
