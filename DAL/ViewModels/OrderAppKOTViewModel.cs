using DAL.Models;

namespace DAL.ViewModels;

public class OrderAppKOTViewModel
{
    public List<Category> categories { get; set; }
    public string CategoryName { get; set; }
    public int CategoryId { get; set; }
    public List<MarkAsPrepared> markAsPreparedItems { get; set; }

    public List<KOTOrdersListViewModel> kOTOrdersListViewModels { get; set; }
    

    public class KOTOrdersListViewModel
    {
        public int OrderId { get; set; }

        public int SectionID { get; set; }
        public string SectionName { get; set; }
        public TimeSpan Duration { get; set; }

        public List<Table> tables { get; set; }
        public List<KOTItemListViewModel> items { get; set; }
        public string Instruction { get; set; }


    }
    public class KOTItemListViewModel
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public decimal Rate { get; set; }
        public decimal Amount { get; set; }

        public List<KOTModifierListViewModel> modifiers { get; set; }

    }
    public class KOTModifierListViewModel
    {
        public int ModifierId { get; set; }
        public string ModifierName { get; set; }
        public decimal Rate { get; set; }
        public int Quantity { get; set; }
        public string Instruction { get; set; }

    }
    public class MarkAsPrepared
    {
        public int ItemId { get; set; }
        public int Quantity { get; set; }
    }
}
