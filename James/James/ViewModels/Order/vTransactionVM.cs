using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using James.Models;

namespace James.ViewModels
{
    public class vTransactionVM
    {
     
        public int rId { get; set; }
        public decimal total { get; set; }
      


     
        public transaction transaction { get; set; }
        public List<trans> trans { get; set; }


    }
    public class trans
    {
        public int cId { get; set; }
        public string customerName { get; set; }

        public decimal companyIncome { get; set; }
        public string customerEmail { get; set; }
        public DateTime transactionDate { get; set; }
    }

}