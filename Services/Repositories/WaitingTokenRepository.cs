using DAL.Models;
using DAL.ViewModels;
using Services.Interfaces;

namespace Services.Repositories;

public class WaitingTokenRepository:IWaitingTokenRepository
{
    private readonly PizzashopContext _context;
    public WaitingTokenRepository(PizzashopContext context)
    {
        _context = context;
    }

    public WaitingToken GetWaitingToken(int id){
        WaitingToken? waitingToken=_context.WaitingTokens.FirstOrDefault(w=>w.WaitingTokenId==id);
        return waitingToken;
    }

    public void Update(WaitingToken waitingToken){
        _context.WaitingTokens.Update(waitingToken);
        _context.SaveChanges();
    }

    public  List<Table> AvailableTables(int id){
        List<Table> tables=_context.Tables.Where(t=>t.SectionId==id && t.TableStatus=="Available" && t.IsActive==true).ToList();
        return tables;
    }
     public CustomErrorViewModel AssignTable(OrderAppCustomerViewModel orderAppCustomerViewModel)
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
        
        Customer customer = _context.Customers.Where(c => c.Email == waitingToken.Email).FirstOrDefault();
        if (customer != null)
        {
            Table table = _context.Tables.Where(t => t.CurrentCustomerId == customer.CustomerId && t.TableStatus == "Running").FirstOrDefault();
            if (table != null)
            {
                return new CustomErrorViewModel { Message = "This Customer Has A Running Order", Status = false };
            }

            customer.Email = waitingToken.Email;
            customer.FirstName = waitingToken.FirstName;
            customer.LastName = waitingToken.FirstName;
            customer.Phone = waitingToken.Phone ?? customer.Phone;
            customer.NoOfPersons = waitingToken.NoOfPersons;

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
                Email = waitingToken.Email,
                FirstName = waitingToken.FirstName,
                LastName = waitingToken.FirstName,
                Phone = waitingToken.Phone,
                NoOfPersons = waitingToken.NoOfPersons
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
}
