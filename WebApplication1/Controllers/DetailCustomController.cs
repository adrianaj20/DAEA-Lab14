using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DetailCustomController : ControllerBase
    {
        private readonly MyDataContext _context;

        public DetailCustomController(MyDataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public List<Detail> GetDetailsByInvoiceNumber(string invoiceNumber)
        {
            var response = _context.Details
                .Include(x => x.Invoice)
                    .ThenInclude(x => x.Customer)
                .Where(x => x.Invoice.InvoiceNumber == invoiceNumber)
                .OrderBy(x => x.Invoice.Customer.FirstName)
                .ThenBy(x => x.Invoice.InvoiceNumber)
                .ToList();

            return response;
        }

        [HttpGet]
        public List<Detail> GetDetailsByDateRange(DateTime startDate, DateTime endDate)
        {
            var response = _context.Details
            .Include(x => x.Invoice)
                .ThenInclude(x => x.Customer)
            .Include(x => x.Product)
            .Where(x => x.Invoice.Date >= startDate && x.Invoice.Date <= endDate)
            .OrderBy(x => x.Invoice.Date)
            .ThenBy(x => x.Invoice.Customer.FirstName)
            .ThenBy(x => x.Product.Name) 
            .ToList();

            return response;
        }
    }
}
