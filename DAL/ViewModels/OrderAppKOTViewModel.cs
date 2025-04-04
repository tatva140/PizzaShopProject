using DAL.Models;

namespace DAL.ViewModels;

public class OrderAppKOTViewModel
{
    public List<Category> categories { get; set; }

    public List<KOTOrdersListViewModel> kOTOrdersListViewModels { get; set; }

    public class KOTOrdersListViewModel
    {
        public int OrderId { get; set; }

        public int SectionID { get; set; }
        public string SectionName { get; set; }
        public DateTime Duration { get; set; }

       
    }
}
