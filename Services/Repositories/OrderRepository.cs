using DAL.Models;
using DAL.ViewModels;
using Services.Interfaces;
using OfficeOpenXml;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace Services.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly PizzashopContext _context;

    public OrderRepository(PizzashopContext context)
    {
        _context = context;
    }

    public List<OrdersListViewModel> GetOrders(string search, string sortOrder, string status, string time, string from, string to, int pageNumber, int pageSize, out int totalRecords)
    {
        var query = _context.Orders.Where(m => m.IsActive == true).AsQueryable();
        if (from != null && to != null)
        {
            query = query.Where(m => m.CreatedAt >= DateTime.Parse(from) && m.CreatedAt <= DateTime.Parse(to));
        }
        if (search != null)
        {
            query = query.Where(m => m.OrderId.ToString().Contains(search));
        }
        if (status != null)
        {
            query = query.Where(m => m.OrderStatus.ToLower().Contains(status.ToLower()));
        }
        if (time != null)
        {
            query = query.Where(m => m.CreatedAt.ToString().Contains(time == "7" ? DateTime.Now.AddDays(-7).ToString() : time == "30" ? DateTime.Now.AddDays(-30).ToString() : DateTime.Now.AddDays(-365).ToString()));
        }



        var query1 = (from o in query
                      join c in _context.Customers on o.CustomerId equals c.CustomerId
                      join r in _context.Reviews on o.OrderId equals r.OrderId into reviews
                      from r in reviews.DefaultIfEmpty()
                      join p in _context.Payments on o.OrderId equals p.OrderId into payments
                      from p in payments.DefaultIfEmpty()
                      select new OrdersListViewModel
                      {
                          OrderId = o.OrderId,
                          PaymentMode = p.PaymentMode ?? "",
                          CustomerName = c.FirstName,
                          CreatedAt = o.CreatedAt,
                          TotalAmount = o.TotalAmount,
                          OrderStatus = o.OrderStatus,
                          Rating = (r.Ambience + r.Food + r.Service + r.Service) / 3 ?? 0
                      });
        switch (sortOrder)
        {
            case "orderIddesc":
                query1 = query1.OrderByDescending(u => u.OrderId);
                break;
            case "datesortdesc":
                query1 = query1.OrderByDescending(u => u.CreatedAt);
                break;
            case "customersortdesc":
                query1 = query1.OrderByDescending(u => u.CustomerName);
                break;
            case "amtsortdesc":
                query1 = query1.OrderByDescending(u => u.TotalAmount);
                break;
            case "orderId":
                query1 = query1.OrderBy(u => u.OrderId);
                break;
            case "datesort":
                query1 = query1.OrderBy(u => u.CreatedAt);
                break;
            case "customersort":
                query1 = query1.OrderBy(u => u.CustomerName);
                break;
            case "amtsort":
                query1 = query1.OrderBy(u => u.TotalAmount);
                break;
            default:
                query1 = query1.OrderBy(u => u.OrderId);
                break;
        }
        totalRecords = query.Count();
        return query1.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
    }

    public FileContentResult UploadExcel(string search, string status, string time, string from, string to)
    {

        // Build the same query as in Index (without pagination)
        var query = _context.Orders.Where(m => m.IsActive == true).AsQueryable();
        if (search != null)
        {
            query = query.Where(m => m.OrderId.ToString().Contains(search));
        }
        if (status != null)
        {
            query = query.Where(m => m.OrderStatus.ToLower().Contains(status.ToLower()));
        }
        if (time != null)
        {
            query = query.Where(m => m.CreatedAt.ToString().Contains(time == "7" ? DateTime.Now.AddDays(-7).ToString() : time == "30" ? DateTime.Now.AddDays(-30).ToString() : DateTime.Now.AddDays(-365).ToString()));
        }
        if (from != null && to != null)
        {
            query = query.Where(m => m.CreatedAt >= DateTime.Parse(from) && m.CreatedAt <= DateTime.Parse(to));
        }

        var orders = query.OrderBy(e => e.OrderId);
        List<OrdersListViewModel> query1 = (from o in orders
                                            join c in _context.Customers on o.CustomerId equals c.CustomerId
                                            join r in _context.Reviews on o.OrderId equals r.OrderId into reviews
                                            from r in reviews.DefaultIfEmpty()
                                            select new OrdersListViewModel
                                            {
                                                OrderId = o.OrderId,
                                                PaymentMode = o.PaymentMode,
                                                CustomerName = c.FirstName,
                                                CreatedAt = o.CreatedAt,
                                                TotalAmount = o.TotalAmount,
                                                OrderStatus = o.OrderStatus,
                                                Rating = (r.Ambience + r.Food + r.Service + r.Service) / 3 ?? 0
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
            statusLabelCell.Value = "Status:";
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
            statusCell.Value = status ?? "All Status";
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
            worksheet.Cells[9, 1].Value = "Order ID";
            worksheet.Cells[9, 2].Value = "Date";
            worksheet.Cells[9, 3].Value = "Customer";
            worksheet.Cells[9, 4].Value = "Status";
            worksheet.Cells[9, 5].Value = "Payment Mode";
            worksheet.Cells[9, 6].Value = "Rating";
            worksheet.Cells[9, 7].Value = "Total Amount";
            int row = 10;
            foreach (var e in query1)
            {
                worksheet.Cells[row, 1].Value = e.OrderId;
                worksheet.Cells[row, 2].Value = e.CreatedAt.ToString();
                worksheet.Cells[row, 3].Value = e.CustomerName;
                worksheet.Cells[row, 4].Value = e.OrderStatus;
                worksheet.Cells[row, 5].Value = e.PaymentMode;
                worksheet.Cells[row, 6].Value = e.Rating;
                worksheet.Cells[row, 7].Value = e.TotalAmount;

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

    public OrdersListViewModel GetOrderDetails(int id)
    {
        var query = (from o in _context.Orders
                     join c in _context.Customers on o.CustomerId equals c.CustomerId
                     join r in _context.Reviews on o.OrderId equals r.OrderId into reviews
                     join t in _context.AllocatedTables on o.CustomerId equals t.CustomerId
                     join s in _context.Tables on t.TableId equals s.TableId
                     join sec in _context.Sections on s.SectionId equals sec.SectionId
                     from r in reviews.DefaultIfEmpty()
                     join p in _context.Payments on o.OrderId equals p.OrderId into payments
                     from p in payments.DefaultIfEmpty()
                     where o.OrderId == id
                     select new OrdersListViewModel
                     {
                         OrderId = o.OrderId,
                         CustomerName = c.FirstName,
                         Email = c.Email,
                         SectionName = sec.SectionName,
                         PhoneNumber = c.Phone ?? "",
                         NoOfPersons = c.NoOfPersons ?? 0,
                         CreatedAt = o.CreatedAt,
                         SubTotal = o.SubTotal,
                         OrderStatus = o.OrderStatus ?? "",
                         TotalAmount = o.TotalAmount,
                         UpdatedAt = o.UpdatedAt,
                         Rating = (r.Ambience + r.Food + r.Service + r.Service) / 3 ?? 0,
                         TableName = (from t in _context.AllocatedTables
                                      join s in _context.Tables on t.TableId equals s.TableId
                                      where t.CustomerId == o.CustomerId
                                      select s.TableName).ToList(),
                         itemLists = (from i in _context.OrderItems
                                      join it in _context.Items on i.ItemId equals it.ItemId
                                      where i.OrderId == id
                                      select new OrderItemListViewModel
                                      {
                                          Quantity = i.Quantity,
                                          Rate = i.Rate,
                                          ItemName = it.Name
                                      }).ToList(),
                         modifierLists = (from i in _context.OrderItems
                                          join m in _context.OrderModifiers on i.ItemId equals m.ItemId
                                          join mod in _context.Modifiers on m.ModifierId equals mod.ModifierId
                                          where m.OrderId == id
                                          select new OrderModifierListViewModel
                                          {
                                              ModifierName = mod.ModifierName,
                                              Quantity = m.Quantity,
                                              Rate = m.Rate
                                          }).ToList(),
                         taxLists = (from i in _context.Orders
                                     join t in _context.OrderTaxes on i.OrderId equals t.OrderId
                                     join tax in _context.Taxes on t.TaxId equals tax.TaxId
                                     where t.OrderId == id
                                     select new OrderTaxListViewModel
                                     {
                                         TaxName = tax.TaxName,
                                         Amount = t.TaxAmount
                                     }).ToList(),
                         PaymentMode = p.PaymentMode ?? "",
                         PaidOn = p.CreatedAt,
                         OrderDuration = o.Duration,

                     }
        );

        return query.FirstOrDefault();
    }
}

