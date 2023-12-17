using WebApplication1.Models;

namespace WebApplication1.DTOs
{
    public class InvoiceDTO
    {
        public DateTime Date { get; set; }
        public string InvoiceNumber { get; set; }
        public float Total { get; set; }

        public int CustomerId { get; set; }

    }
}
