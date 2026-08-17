using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceExtractor.Model
{
    public class Invoice
    {
        public string InvoiceNumber { get; set; } = "";
        public string BillingDate { get; set; } = "";
        public List<InvoiceItem> Items { get; set; } = new();

        public decimal SubTotal { get; set; }
        public decimal GstPercentage { get; set; }
        public decimal GstAmount { get; set; }
        public decimal TotalAmount { get; set; }
    }

    public class InvoiceItem
    {
        public string ProductName { get; set; } = "";
        public string SerialNumber { get; set; } = "";
        public int Quantity { get; set; }
        public decimal UnitCost { get; set; }
        public decimal Amount { get; set; }
    }

    public class InvoiceValidationResult
    {
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; } = new();
    }
}
