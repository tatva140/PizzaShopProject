using System.ComponentModel.DataAnnotations;

namespace DAL.ViewModels;

public class OrdersViewModel
{
     public int OrderId { get; set; }

    public int? CustomerId { get; set; }

    public decimal? TotalAmount { get; set; }

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


}
