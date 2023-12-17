using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DTOs;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CustomerCustomController : ControllerBase
    {
        private readonly MyDataContext _context;

        public CustomerCustomController(MyDataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public List<Customer> GetProductsListFilterNameOrLastName(string value)
        {
            var response = _context.Customers
                .Where(x => x.FirstName.Contains(value) || x.LastName.Contains(value))
                .OrderByDescending(x => x.LastName)
                .ToList();

            return response;
        }

        [HttpPost]
        public void CreateCustomer(CustomerDTO customerDTO)
        {
            var newCustomer = new Customer
            {
                FirstName = customerDTO.FirstName,
                LastName = customerDTO.LastName,
                DocumentNumber = customerDTO.DocumentNumber,
                IsActive = true
            };

            _context.Customers.Add(newCustomer);
            _context.SaveChanges();
        }

        [HttpDelete("{id}")]
        public void DeleteCustomer(int id)
        {
            var customer = _context.Customers.Find(id);
            customer.IsActive = false;
            _context.Entry(customer).State = EntityState.Modified;
            _context.SaveChanges();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCustomerDocument(int id, [FromBody] CustomerUpdateDocumentDTO customerDTO)
        {
            var customer = _context.Customers.Find(id);

            customer.DocumentNumber = customerDTO.DocumentNumber;
            _context.SaveChanges();

            return Ok(customer);
        }

        [HttpPost("{id}")]
        public IActionResult InsertarListaFacturasPorCliente(int id, [FromBody] List<ListInvoicesForCustomerDTO> invoicesDTO)
        {
            foreach (var invoiceDTO in invoicesDTO)
            {
                var newInvoice = new Invoice
                {
                    CustomerId = id,
                    Date = invoiceDTO.Date,
                    InvoiceNumber = invoiceDTO.InvoiceNumber,
                    Total = invoiceDTO.Total
                };

                _context.Invoices.Add(newInvoice);
            }

            _context.SaveChanges();

            return Ok("Facturas insertadas correctamente");
        }


    }
}
