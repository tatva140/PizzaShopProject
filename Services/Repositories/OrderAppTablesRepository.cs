using DAL.Models;
using DAL.ViewModels;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using DAL.ViewModels; // Ensure this namespace contains SectionViewModel
using Services.Interfaces;
using static DAL.ViewModels.OrderAppTablesViewModel;
using Microsoft.EntityFrameworkCore;

namespace Services.Repositories;

public class OrderAppTablesRepository : IOrderAppTablesRepository
{
    private readonly PizzashopContext _context;

    public OrderAppTablesRepository(PizzashopContext context)
    {
        _context = context;
    }

    public OrderAppTablesViewModel GetTablesAndSections(List<Section> sections)
    {
        OrderAppTablesViewModel orderAppTablesViewModel = new OrderAppTablesViewModel();

        orderAppTablesViewModel.Sections = _context.Sections.Where(s => s.IsActive == true).Select(section => new SectionListViewModel
        {
            SectionId = section.SectionId,
            SectionName = section.SectionName,
            Available = section.Tables.Where(t => t.IsActive == true).Count(t => t.TableStatus == "Available"),
            Assigned = section.Tables.Where(t => t.IsActive == true).Count(t => t.TableStatus == "Assigned"),
            Running = section.Tables.Where(t => t.IsActive == true).Count(t => t.TableStatus == "Running"),
            Selected = section.Tables.Where(t => t.IsActive == true).Count(t => t.TableStatus == "Selected"),
            Tables = section.Tables.Where(s => s.IsActive == true).Select(table => new OrderAppTableListViewModel
            {
                TableId = table.TableId,
                TableName = table.TableName,
                Status = table.TableStatus,
                Capacity = table.Capacity ?? 0,
                TotalAmount = _context.Orders.Where(order => order.OrderId == table.CurrentOrderId).Select(o => o.TotalAmount).FirstOrDefault(),
                Duration = _context.Orders.Where(order => order.OrderId == table.CurrentOrderId).Select(o => o.Duration).FirstOrDefault().HasValue ? (DateTime.Now - _context.Orders.Where(order => order.OrderId == table.CurrentOrderId).Select(o => o.Duration).FirstOrDefault().Value) : TimeSpan.Zero
            }).ToList(),


        }).ToList();
        return orderAppTablesViewModel;
    }

    public WaitingTokenListViewModel WaitingTokenList(int id)
    {

        List<WaitingToken> waitingTokens;
        if (id == 0)
        {
            waitingTokens = (from w in _context.WaitingTokens
                             where w.IsActive == true && w.Isassign == false
                             select new WaitingToken
                             {
                                 WaitingTokenId = w.WaitingTokenId,
                                 FirstName = w.FirstName,
                                 LastName = w.LastName,
                                 NoOfPersons = w.NoOfPersons ?? 0,
                                 Email = w.Email,
                                 CreatedAt = w.CreatedAt,
                                 Phone = w.Phone
                             }).ToList();
        }
        else
        {
            waitingTokens = (from w in _context.WaitingTokens
                             where w.IsActive == true && w.Isassign == false && w.SectionId == id
                             select new WaitingToken
                             {
                                 WaitingTokenId = w.WaitingTokenId,
                                 FirstName = w.FirstName,
                                 LastName = w.LastName,
                                 NoOfPersons = w.NoOfPersons ?? 0,
                                 Email = w.Email,
                                 CreatedAt = w.CreatedAt,
                                 Phone = w.Phone
                             }).ToList();
        }
        WaitingTokenListViewModel waitingTokenListViewModel = new WaitingTokenListViewModel
        {
            waitingTokens = waitingTokens
        };
        return waitingTokenListViewModel;
    }

    public OrderAppCustomerViewModel CustomerDetails(string email)
    {
        Customer customer = _context.Customers.FirstOrDefault(c => c.Email == email && c.IsActive == true);
        if (customer == null) return null;

        OrderAppCustomerViewModel orderAppCustomerViewModel = new OrderAppCustomerViewModel
        {
            EmailAddress = customer.Email,
            FirstName = customer.FirstName,
            Phone = customer.Phone ?? "",
            NoOfPersons = customer.NoOfPersons ?? 0
        };
        return orderAppCustomerViewModel;
    }
    public OrderAppCustomerViewModel WaitingTokenCustomerDetails(int id)
    {
        WaitingToken customer = _context.WaitingTokens.FirstOrDefault(c => c.WaitingTokenId == id && c.IsActive == true);
        if (customer == null) return null;
        Section section=_context.Sections.Where(s=>s.SectionId==customer.SectionId).FirstOrDefault();
        OrderAppCustomerViewModel orderAppCustomerViewModel = new OrderAppCustomerViewModel
        {
            EmailAddress = customer.Email,
            FirstName = customer.FirstName,
            NoOfPersons = customer.NoOfPersons ?? 0,
            Phone = customer.Phone ?? "",
            SectionId=customer.SectionId??0,
            SectionName=section.SectionName
        };
        return orderAppCustomerViewModel;
    }

    public CustomErrorViewModel AssignTable(OrderAppCustomerViewModel orderAppCustomerViewModel)
    {
        if (orderAppCustomerViewModel.waitingTokenId != 0)
        {

            WaitingToken waitingToken = _context.WaitingTokens.Find(orderAppCustomerViewModel.waitingTokenId);
            if (waitingToken == null)
            {
                return new CustomErrorViewModel { Message = "No waiting Token", Status = false };
            }
            else
            {
                waitingToken.Isassign = true;
                waitingToken.SectionId = orderAppCustomerViewModel.SectionId;
                _context.SaveChanges();
            }
        }
        Customer customer = _context.Customers.Where(c => c.Email == orderAppCustomerViewModel.EmailAddress).FirstOrDefault();
        if (customer != null)
        {
            Table table = _context.Tables.Where(t => t.CurrentCustomerId == customer.CustomerId && t.TableStatus == "Running").FirstOrDefault();
            if (table != null)
            {
                return new CustomErrorViewModel { Message = "This Customer Has A Running Order", Status = false };
            }

            customer.Email = orderAppCustomerViewModel.EmailAddress;
            customer.FirstName = orderAppCustomerViewModel.FirstName;
            customer.LastName = orderAppCustomerViewModel.FirstName;
            customer.Phone = orderAppCustomerViewModel.Phone ?? customer.Phone;
            customer.NoOfPersons = orderAppCustomerViewModel.NoOfPersons;

            foreach (var tables in orderAppCustomerViewModel.selectedTables)
            {
                Table table1 = _context.Tables.Find(tables);
                table1.CurrentCustomerId = customer.CustomerId;
                table1.TableStatus = "Assigned";
            }
            _context.SaveChanges();
        }
        else
        {
            Customer customer1 = new Customer
            {
                Email = orderAppCustomerViewModel.EmailAddress,
                FirstName = orderAppCustomerViewModel.FirstName,
                LastName = orderAppCustomerViewModel.FirstName,
                Phone = orderAppCustomerViewModel.Phone,
                NoOfPersons = orderAppCustomerViewModel.NoOfPersons
            };
            _context.Customers.Add(customer1);
            _context.SaveChanges();
            foreach (var table in orderAppCustomerViewModel.selectedTables)
            {
                Table table2 = _context.Tables.Find(table);
                table2.CurrentCustomerId = customer1.CustomerId;
                table2.TableStatus = "Assigned";

            }
            _context.SaveChanges();

        }
        return new CustomErrorViewModel() { Message = "Assigned Table", Status = true };

    }

    public CustomErrorViewModel AddWaitingToken(OrderAppCustomerViewModel orderAppCustomerViewModel)
    {
        WaitingToken waitingToken = _context.WaitingTokens.Where(w => w.Email == orderAppCustomerViewModel.EmailAddress).FirstOrDefault();
        if (waitingToken != null && orderAppCustomerViewModel.EditFlag == false)
        {
            if (waitingToken.Isassign == false)
            {
                return new CustomErrorViewModel { Message = "A waiting Token has already been generated for this customer!", Status = false };
            }
        }
        else if (waitingToken != null && orderAppCustomerViewModel.EditFlag == true)
        {
            waitingToken.FirstName = orderAppCustomerViewModel.FirstName;
            waitingToken.LastName = orderAppCustomerViewModel.FirstName;
            waitingToken.NoOfPersons = orderAppCustomerViewModel.NoOfPersons;
            waitingToken.Phone = orderAppCustomerViewModel.Phone;
            waitingToken.SectionId = orderAppCustomerViewModel.SectionId;
            waitingToken.Isassign = false;
            return new CustomErrorViewModel() { Message = "Waiting Token Edited", Status = true };

        }
        else
        {
            WaitingToken waitingToken1 = new WaitingToken
            {
                FirstName = orderAppCustomerViewModel.FirstName,
                LastName = orderAppCustomerViewModel.FirstName,
                Phone = orderAppCustomerViewModel.Phone,
                Email = orderAppCustomerViewModel.EmailAddress,
                NoOfPersons = orderAppCustomerViewModel.NoOfPersons,
                SectionId = orderAppCustomerViewModel.SectionId,
                Isassign = false,
            };
            _context.WaitingTokens.Add(waitingToken1);
        }
        _context.SaveChanges();
        return new CustomErrorViewModel() { Message = "Waiting Token Added", Status = true };
    }
}
