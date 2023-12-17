using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DTOs;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class InvoiceCustomController : ControllerBase
    {
        private readonly MyDataContext _context;

        public InvoiceCustomController(MyDataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public List<Invoice> GetInvoicesByCustomerName(string customerName)
        {
            var response = _context.Invoices
                .Include(x => x.Customer)
                //.Where(x => x.Customer.FirstName == customerName)
                .Where(x => x.Customer.FirstName.Contains(customerName))
                .OrderByDescending(i => i.Customer.FirstName)
                .ToList();

            return response;
        }

        [HttpPost]
        public void CreateInvoice(InvoiceDTO invoiceDTO)
        {
            var newInvoice = new Invoice
            {
                CustomerId = invoiceDTO.CustomerId,
                Date = invoiceDTO.Date,
                InvoiceNumber = invoiceDTO.InvoiceNumber,
                Total = invoiceDTO.Total
            };

            _context.Invoices.Add(newInvoice);
            _context.SaveChanges();
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> InsertInvoiceDetails(int id, List<ListDetailsForInvoiceDTO> details)
        {
            foreach (var detailDto in details)
            {
                var detail = new Detail
                {
                    InvoiceId = id,
                    Amount = detailDto.Amount,
                    Price = detailDto.Price,
                    SubTotal = detailDto.SubTotal,
                    ProductId = detailDto.ProductId
                    
                };

                _context.Details.Add(detail);
            }
            await _context.SaveChangesAsync();
            return Ok("Detalles de factura insertados correctamente.");
        }

    }
}
