using DAL.Models;
using DAL.ViewModels;
using Services.Interfaces;
using OfficeOpenXml;
using Microsoft.AspNetCore.Mvc;

namespace Services.Repositories;

public class OrderRepository:IOrderRepository
{
    private readonly PizzashopContext _context;
    
    public OrderRepository(PizzashopContext context)
    {
        _context = context;
    }

     public List<OrdersViewModel> GetOrders(string search,string status,string time,string from,string to, int pageNumber, int pageSize, out int totalRecords)
    {
         var query = _context.Orders.Where(m => m.IsActive == true).AsQueryable();
            if (search!=null)
            {
                query = query.Where(m => m.OrderId.ToString().Contains(search));
            }
            if (status!=null)
            {
                query = query.Where(m => m.OrderStatus.ToLower().Contains(status.ToLower()));
            }
            if (time!=null)
            {
                query = query.Where(m => m.CreatedAt.ToString().Contains(time=="7"?DateTime.Now.AddDays(-7).ToString():time=="30"?DateTime.Now.AddDays(-30).ToString():DateTime.Now.AddDays(-365).ToString()));
            }
            if (from != null && to!=null)
            {
                query = query.Where(m => m.CreatedAt>=DateTime.Parse(from) && m.CreatedAt<=DateTime.Parse(to));
            }
           
            var orders =  query.OrderBy(e => e.OrderId);
             List<OrdersViewModel> query1 =(from o in orders
                        join c in _context.Customers on o.CustomerId equals c.CustomerId
                        join r in _context.Reviews on o.OrderId equals r.OrderId into reviews
                        from r in reviews.DefaultIfEmpty()
                        select new OrdersViewModel
                        {
                            OrderId = o.OrderId,
                            PaymentMode=o.PaymentMode,
                            CustomerName = c.FirstName,
                            CreatedAt = o.CreatedAt,
                            TotalAmount = o.TotalAmount,
                            OrderStatus = o.OrderStatus,
                            Rating = (r.Ambience+r.Food+r.Service+r.Service)/3 ??0
                        }).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                        totalRecords = query.Count();
        return query1;
    }

    public FileContentResult UploadExcel(string search,string status,string time,string from,string to){
        
            // Build the same query as in Index (without pagination)
            var query = _context.Orders.Where(m => m.IsActive == true).AsQueryable();
            if (search!=null)
            {
                query = query.Where(m => m.OrderId.ToString().Contains(search));
            }
            if (status!=null)
            {
                query = query.Where(m => m.OrderStatus.ToLower().Contains(status.ToLower()));
            }
            if (time!=null)
            {
                query = query.Where(m => m.CreatedAt.ToString().Contains(time=="7"?DateTime.Now.AddDays(-7).ToString():time=="30"?DateTime.Now.AddDays(-30).ToString():DateTime.Now.AddDays(-365).ToString()));
            }
            if (from != null && to!=null)
            {
                query = query.Where(m => m.CreatedAt>=DateTime.Parse(from) && m.CreatedAt<=DateTime.Parse(to));
            }
           
            var orders =  query.OrderBy(e => e.OrderId);
             List<OrdersViewModel> query1 =(from o in orders
                        join c in _context.Customers on o.CustomerId equals c.CustomerId
                        join r in _context.Reviews on o.OrderId equals r.OrderId into reviews
                        from r in reviews.DefaultIfEmpty()
                        select new OrdersViewModel
                        {
                            OrderId = o.OrderId,
                            PaymentMode=o.PaymentMode,
                            CustomerName = c.FirstName,
                            CreatedAt = o.CreatedAt,
                            TotalAmount = o.TotalAmount,
                            OrderStatus = o.OrderStatus,
                            Rating = (r.Ambience+r.Food+r.Service+r.Service)/3 ??0
                        }).ToList();
            // Use EPPlus to generate an Excel file.
            // IMPORTANT: Set the EPPlus license context to non-commercial.
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            // Create the Excel package and worksheet
            using (var package = new ExcelPackage())
            {
                // Create a worksheet.
                var worksheet = package.Workbook.Worksheets.Add("Employees");
                // Set up header row.
                worksheet.Cells[1, 1].Value = "Order ID";
                worksheet.Cells[1, 2].Value = "Date";
                worksheet.Cells[1, 3].Value = "Customer";
                worksheet.Cells[1, 4].Value = "Status";
                worksheet.Cells[1, 5].Value = "Payment Mode";
                worksheet.Cells[1, 6].Value = "Rating";
                worksheet.Cells[1, 7].Value = "Total Amount";
                int row = 2;
                foreach (var e in query1)
                {
                    worksheet.Cells[row, 1].Value = e.OrderId;
                    worksheet.Cells[row, 2].Value =e.CreatedAt.ToString();
                    worksheet.Cells[row, 3].Value = e.CustomerName;
                    worksheet.Cells[row, 4].Value = e.OrderStatus;
                    worksheet.Cells[row, 5].Value = e.PaymentMode;
                    worksheet.Cells[row, 6].Value = e.Rating;
                    worksheet.Cells[row, 7].Value = e.TotalAmount;

                    row++;
                }
                // Auto-fit columns for better readability
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
                // Convert the package to a byte array
                var fileBytes = package.GetAsByteArray();
                // Return the Excel file for download
                // 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' is the MIME type for Excel files
                return new FileContentResult(
                    fileBytes, //Excel File data in Byte Array
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" //Excel Sheet Mime Type
                )
                {
                    FileDownloadName = "Orders.xlsx" //File Name
                };
            }
        }
    }

