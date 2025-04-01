namespace DAL.ViewModels;

public class OrderAppTablesViewModel
{

    public List<SectionListViewModel> Sections { get; set; }


    public class OrderAppTableListViewModel
    {
        public int TableId { get; set; }

        public string TableName { get; set; }
        public string Status { get; set; }

        public int Capacity { get; set; }
        public decimal? TotalAmount { get; set; }
        public TimeSpan Duration { get; set; }

    }
    public class SectionListViewModel
    {
        public string SectionName { get; set; }
        public int Available { get; set; }
        public int Selected { get; set; }
        public int Running { get; set; }
        public int Assigned { get; set; }
        public int SectionId { get; set; }

        public List<OrderAppTableListViewModel> Tables { get; set; }

    }



}
