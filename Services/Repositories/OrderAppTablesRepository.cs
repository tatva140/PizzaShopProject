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
            Available = section.Tables.Count(t => t.TableStatus == "Available"),
            Assigned = section.Tables.Count(t => t.TableStatus == "Occupied"),
            Running = section.Tables.Count(t => t.TableStatus == "Running"),
            Selected = section.Tables.Count(t => t.TableStatus == "Selected"),
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
        List<WaitingToken> waitingTokens = (from w in _context.WaitingTokens
                                            where w.IsActive == true && w.Isassign == false && w.SectionId==id
                                            select new WaitingToken
                                            {
                                                WaitingTokenId = w.WaitingTokenId,
                                                FirstName = w.FirstName,
                                                LastName = w.LastName,
                                                NoOfPersons = w.NoOfPersons ?? 0,
                                                Email = w.Email
                                            }).ToList();
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
            LastName = customer.LastName,
            Phone = customer.Phone,
            NoOfPersons = customer.NoOfPersons ?? 0
        };
        return orderAppCustomerViewModel;
    }
    public OrderAppCustomerViewModel WaitingTokenCustomerDetails(string email)
    {
        WaitingToken customer = _context.WaitingTokens.FirstOrDefault(c => c.Email == email && c.IsActive == true);
        if (customer == null) return null;
        OrderAppCustomerViewModel orderAppCustomerViewModel = new OrderAppCustomerViewModel
        {
            EmailAddress = customer.Email,
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            NoOfPersons = customer.NoOfPersons ?? 0
        };
        return orderAppCustomerViewModel;
    }

    public CustomErrorViewModel AssignTable(AssignCustomerTablesViewModel assignCustomerTablesViewModel)
    {
        if (assignCustomerTablesViewModel.waitingTokenId != 0)
        {

            WaitingToken waitingToken = _context.WaitingTokens.Find(assignCustomerTablesViewModel.waitingTokenId);
            if (waitingToken == null)
            {
                return new CustomErrorViewModel { Message = "No waiting Token", Status = false };
            }
            else
            {
                waitingToken.Isassign = true;
            }
        }
        Customer customer = _context.Customers.Where(c => c.Email == assignCustomerTablesViewModel.email).FirstOrDefault();
        if (customer != null)
        {
            Table table = _context.Tables.Where(t => t.CurrentCustomerId == customer.CustomerId && t.TableStatus == "Running").FirstOrDefault();
            if (table != null)
            {
                return new CustomErrorViewModel { Message = "This Customer Has A Running Order", Status = false };
            }

            customer.Email = assignCustomerTablesViewModel.email;
            customer.FirstName = assignCustomerTablesViewModel.name;
            customer.Phone = assignCustomerTablesViewModel.phone ?? customer.Phone;
            customer.NoOfPersons = assignCustomerTablesViewModel.noOfPersons;

            foreach (var tables in assignCustomerTablesViewModel.selectedTables)
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
                Email = assignCustomerTablesViewModel.email,
                FirstName = assignCustomerTablesViewModel.name,
                LastName = assignCustomerTablesViewModel.name,
                Phone = assignCustomerTablesViewModel.phone,
                NoOfPersons = assignCustomerTablesViewModel.noOfPersons
            };
            _context.Customers.Add(customer1);
            _context.SaveChanges();
            foreach (var table in assignCustomerTablesViewModel.selectedTables)
            {
                Table table2 = _context.Tables.Find(table);
                table2.CurrentCustomerId = customer1.CustomerId;
                table2.TableStatus = "Assigned";

            }
            _context.SaveChanges();

        }
        return new CustomErrorViewModel() { Message = "Added", Status = true };

    }
}
