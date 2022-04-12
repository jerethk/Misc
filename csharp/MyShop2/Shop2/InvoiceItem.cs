using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopDB2
{
    class InvoiceItem
    {
        public int recordId { get; set; }
        public int invoice { get; set; }
        public string product { get; set; }
    }
}
