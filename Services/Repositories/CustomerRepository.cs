using DAL.Models;
using DAL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using Services.Interfaces;

namespace Services.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly PizzashopContext _context;
    public CustomerRepository(PizzashopContext context)
    {
        _context = context;
    }
    public List<CustomerListViewModel> GetCustomers(string search,string sortOrder, string time, string from, string to, int pageNumber, int pageSize, out int totalRecords)
    {
        var query = _context.Customers.Where(m => m.IsActive == true).AsQueryable();
        if (from != null && to != null)
        {
            query = query.Where(m => m.CreatedAt >= DateTime.Parse(from) && m.CreatedAt <= DateTime.Parse(to));
        }
        if (search != null)
        {
            query = query.Where(m => m.FirstName.ToLower().Contains(search.ToLower()));
        }

        if (time != null && time != "custom")
        {
            query = query.Where(m => time == "7" ? m.CreatedAt.Value.Date >= DateTime.Now.Date.AddDays(-7): 
                                      time == "30" ? m.CreatedAt.Value.Date >= DateTime.Now.Date.AddDays(-30) : 
                                      time == "today" ? m.CreatedAt.Value.Date == DateTime.Now.Date : 
                                      time == "CurrMonth" ? m.CreatedAt.Value.Month == DateTime.Now.Month && m.CreatedAt.Value.Year == DateTime.Now.Year : 
                                      m.CreatedAt.Value.Date >= DateTime.Now.Date.AddDays(-365).ToUniversalTime());
        }

        var customers = query.OrderBy(e => e.CustomerId);
        var query1 = (from c in customers

                                              select new CustomerListViewModel
                                              {
                                                CustomerId=c.CustomerId,
                                                  CustomerFirstName = c.FirstName,
                                                  CustomerLastName = c.LastName,
                                                  Phone = c.Phone,
                                                  Email = c.Email,
                                                  Date = c.CreatedAt ?? DateTime.Now,
                                                  TotalOrders = (from o in _context.Orders where o.CustomerId == c.CustomerId select o.OrderId).Count(),
                                              });
        switch (sortOrder)
        {
            case "namesortdesc":
                query1 = query1.OrderByDescending(u => u.CustomerFirstName);
                break;
            case "datesortdesc":
                query1 = query1.OrderByDescending(u => u.Date);
                break;
            case "totalordersortdesc":
                query1 = query1.OrderByDescending(u => u.TotalOrders);
                break;
            case "namesort":
                query1 = query1.OrderBy(u => u.CustomerFirstName);
                break;
            case "datesort":
                query1 = query1.OrderBy(u => u.Date);
                break;
            case "totalordersort":
                query1 = query1.OrderBy(u => u.TotalOrders);
                break;
            default:
                query1 = query1.OrderBy(u => u.CustomerFirstName);
                break;
        }
        if(query1!=null)
        {
        totalRecords = query1.Count();

        }else{
            totalRecords = 0;
        }
        return query1.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
    }
    public FileContentResult UploadExcel(string search, string time, string from, string to)
    {

        // Build the same query as in Index (without pagination)
        var query = _context.Customers.Where(m => m.IsActive == true).AsQueryable();
        if (from != null && to != null)
        {
            query = query.Where(m => m.CreatedAt >= DateTime.Parse(from) && m.CreatedAt <= DateTime.Parse(to));
        }
        if (search != null)
        {
            query = query.Where(m => m.FirstName.ToLower().Contains(search.ToLower()));
        }

        if (time != null && time != "custom")
        {
            query = query.Where(m => m.CreatedAt.ToString().Contains(time == "7" ? DateTime.Now.AddDays(-7).ToString() : time == "30" ? DateTime.Now.AddDays(-30).ToString() : time == "today" ? DateTime.UtcNow.ToString() : DateTime.Now.AddDays(-365).ToString()));
        }

        var customers = query.OrderBy(e => e.CustomerId);
        List<CustomerListViewModel> query1 = (from c in customers

                                              select new CustomerListViewModel
                                              {
                                                  CustomerId = c.CustomerId,
                                                  CustomerFirstName = c.FirstName,
                                                  CustomerLastName = c.LastName,
                                                  Phone = c.Phone,
                                                  Email = c.Email,
                                                  Date = c.CreatedAt ?? DateTime.Now,
                                                  TotalOrders = (from o in _context.Orders where o.CustomerId == c.CustomerId select o.OrderId).Count(),
                                              }).ToList();
        // Use EPPlus to generate an Excel file.
        // IMPORTANT: Set the EPPlus license context to non-commercial.
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        string filePath = "C:\\Users\\pca140\\Downloads\\Orders.xlsx";
        // Create the Excel package and worksheet
        using (var package = new ExcelPackage())
        {
            // Create a worksheet.
            var worksheet = package.Workbook.Worksheets.Add("Orders");

            var statusLabelCell = worksheet.Cells["A2:B3"];
            statusLabelCell.Merge = true;
            statusLabelCell.Value = "Account:";
            statusLabelCell.Style.Font.Bold = true;
            statusLabelCell.Style.Font.Size = 12;
            statusLabelCell.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            statusLabelCell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Blue); // Blue background
            statusLabelCell.Style.Font.Color.SetColor(System.Drawing.Color.White); // White text
            statusLabelCell.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            statusLabelCell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            statusLabelCell.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

            var statusCell = worksheet.Cells["C2:D3"];
            statusCell.Merge = true;
            statusCell.Value = "";
            statusCell.Style.Font.Bold = true;
            statusCell.Style.Font.Size = 12;
            statusCell.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            statusCell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            statusCell.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

            var SEARCHLabelCell = worksheet.Cells["F2:G3"];
            SEARCHLabelCell.Merge = true;
            SEARCHLabelCell.Value = "Search Text:";
            SEARCHLabelCell.Style.Font.Bold = true;
            SEARCHLabelCell.Style.Font.Size = 12;
            SEARCHLabelCell.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            SEARCHLabelCell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Blue); // Blue background
            SEARCHLabelCell.Style.Font.Color.SetColor(System.Drawing.Color.White); // White text
            SEARCHLabelCell.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            SEARCHLabelCell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            SEARCHLabelCell.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

            var SEARCHCell = worksheet.Cells["H2:I3"];
            SEARCHCell.Merge = true;
            SEARCHCell.Value = search ?? "";
            SEARCHCell.Style.Font.Bold = true;
            SEARCHCell.Style.Font.Size = 12;
            SEARCHCell.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            SEARCHCell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            SEARCHCell.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

            var dateLabelCell = worksheet.Cells["A6:B7"];
            dateLabelCell.Merge = true;
            dateLabelCell.Value = "Date:";
            dateLabelCell.Style.Font.Bold = true;
            dateLabelCell.Style.Font.Size = 12;
            dateLabelCell.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            dateLabelCell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Blue); // Blue background
            dateLabelCell.Style.Font.Color.SetColor(System.Drawing.Color.White); // White text
            dateLabelCell.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            dateLabelCell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            dateLabelCell.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

            var dateCell = worksheet.Cells["C6:D7"];
            dateCell.Merge = true;
            dateCell.Value = time ?? "All Time";
            dateCell.Style.Font.Bold = true;
            dateCell.Style.Font.Size = 12;
            dateCell.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            dateCell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            dateCell.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

            var totalRecordsLabelCell = worksheet.Cells["F6:G7"];
            totalRecordsLabelCell.Merge = true;
            totalRecordsLabelCell.Value = "No. Of Records:";
            totalRecordsLabelCell.Style.Font.Bold = true;
            totalRecordsLabelCell.Style.Font.Size = 12;
            totalRecordsLabelCell.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            totalRecordsLabelCell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Blue); // Blue background
            totalRecordsLabelCell.Style.Font.Color.SetColor(System.Drawing.Color.White); // White text
            totalRecordsLabelCell.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            totalRecordsLabelCell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            totalRecordsLabelCell.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

            var totalRecordsCell = worksheet.Cells["H6:I7"];
            totalRecordsCell.Merge = true;
            totalRecordsCell.Value = query1.Count() != 0 ? query1.Count().ToString() : "0";
            totalRecordsCell.Style.Font.Bold = true;
            totalRecordsCell.Style.Font.Size = 12;
            totalRecordsCell.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            totalRecordsCell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            totalRecordsCell.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

            var pathName = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "logos/pizzashop_logo.png");

            var imageCell = worksheet.Cells["K2:L7"];
            imageCell.Merge = true;

            var picture = worksheet.Drawings.AddPicture("Logo", new FileInfo(pathName));
            imageCell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            picture.SetPosition(0, 0, 10, 0);
            picture.SetSize(100, 100);

            // Set up header row.
            worksheet.Cells[9, 1].Value = "ID";
            worksheet.Cells[9, 2].Value = "Name";
            worksheet.Cells[9, 3].Value = "Email";
            worksheet.Cells[9, 4].Value = "Date";
            worksheet.Cells[9, 5].Value = "Mobile Number";
            worksheet.Cells[9, 6].Value = "Total Orders";
            int row = 10;
            foreach (var e in query1)
            {
                worksheet.Cells[row, 1].Value = e.CustomerId;
                worksheet.Cells[row, 2].Value = e.CustomerFirstName + " " + e.CustomerLastName;
                worksheet.Cells[row, 3].Value = e.Email;
                worksheet.Cells[row, 4].Value = e.Date.ToString("dd/MM/yyyy");
                worksheet.Cells[row, 5].Value = e.Phone;
                worksheet.Cells[row, 6].Value = e.TotalOrders;


                row++;
            }
            // Auto-fit columns for better readability
            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
            package.SaveAs(new FileInfo(filePath));
            // Convert the package to a byte array
            Byte[] fileBytes = package.GetAsByteArray();
            if (fileBytes == null || fileBytes.Length == 0)
            {
                Console.Write("Error in generating Excel File");
            }
            var file = new FileContentResult(
                fileBytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            );
            return file;
        }
    }

    public List<CustomersViewModel> GetHistory(int id)
    {
        var query = (from c in _context.Customers
                     join o in _context.Orders on c.CustomerId equals o.CustomerId into customers
                     from o in customers.DefaultIfEmpty()
                     join p in _context.Payments on o.OrderId equals p.OrderId into payment
                     from p in payment.DefaultIfEmpty()

                     where c.CustomerId == id

                     select new CustomersViewModel
                     {
                        CustomerName=c.FirstName,
                         OrderDate = o.CreatedAt ?? DateTime.Now,
                         Payment = p.PaymentMode,
                         itemsCount = (from i in _context.OrderItems
                                  where i.OrderId == id
                                  select new OrderItemListViewModel
                                  {
                                      OrderItemsId=i.OrderItemsId
                                  }).Count(),
                        Amount=o.TotalAmount??0,
                        comingSince=c.CreatedAt??DateTime.Now,
                        Phone=c.Phone

                     }).ToList();
                     return query;
    }
}
