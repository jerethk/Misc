using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopDB2
{
    class Transaction
    {
        public int invoiceNo { get; set; }
        public DateTime date { get; set; }
        public int customer { get; set; }
        public decimal amount { get; set; }

        public List<InvoiceItem> items { get; set; }

        public Transaction()
        {
            this.items = new List<InvoiceItem>();
        }
    }
}
