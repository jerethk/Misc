using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopDB2
{
    class Customer
    {
        public int customerId { get; set; }      // customer_id
        public string firstName { get; set; }    // firstname
        public string lastName { get; set; }    // lastname
        public string address { get; set; }     // address
        public string postcode { get; set; }    // postcode
        public string state { get; set; }       // state
        public string phone { get; set; }       // phone
        public string email { get; set; }       // email
        public decimal balance { get; set; }        // balance
    }
}
