using System.ComponentModel.DataAnnotations;
using DAL.Models;

namespace DAL.ViewModels;

public class OrdersListViewModel
{
    public int OrderId { get; set; }

    public int? CustomerId { get; set; }

    public decimal? TotalAmount { get; set; }
    public decimal? SubTotal { get; set; }


    public string? PaymentMode { get; set; }

    public string? Instructions { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool? IsActive { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public decimal? TotalTax { get; set; }

    public decimal? TotalDiscount { get; set; }

    public string? OrderStatus { get; set; }

    public string CustomerName { get; set; }

    public int Rating { get; set; }

    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public int SelectedPage { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public int NoOfPersons { get; set; }
    public List<string> TableName { get; set; }
    public string SectionName { get; set; }
    public List<OrderItemListViewModel> itemLists { get; set; }
    public List<OrderTaxListViewModel> taxLists { get; set; }
    public DateTime? PaidOn { get; set; }
    public DateTime? OrderDuration { get; set; }




}
