using System.ComponentModel.DataAnnotations;
using DAL.Models;

namespace DAL.ViewModels;

public class TablesAndSectionViewModel
{
    public List<Table> tables {get;set;}
    public List<Section> sections {get;set;}
      public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public int SelectedPage { get; set; }
     public int SectionId { get; set; }

   [Required]
    [RegularExpression(@"^(?!\s*$).+", ErrorMessage = "Enter Valid  Name.")]
    public string? SectionName { get; set; }

    public string? Description { get; set; }
    public int ShowList { get; set; }

     public int TableId { get; set; }

   [Required]
    [RegularExpression(@"^(?!\s*$).+", ErrorMessage = "Enter Valid  Name.")]
    public string? TableName { get; set; }

   [Required]
    [RegularExpression(@"^[0-9]+$", ErrorMessage = "Enter Valid  Capacity.")]
    public int? Capacity { get; set; }
    public string? TableStatus { get; set; }




}
