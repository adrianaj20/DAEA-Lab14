using WebApplication1.Models;

namespace WebApplication1.DTOs
{
    public class ListInvoicesForCustomerDTO
    {
        public DateTime Date { get; set; }
        public string InvoiceNumber { get; set; }
        public float Total { get; set; }

    }
}
