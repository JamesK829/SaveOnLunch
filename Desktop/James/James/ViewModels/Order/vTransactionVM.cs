using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using James.Models;

namespace James.ViewModels
{
    public class vTransactionVM
    {
        public int cId { get; set; }
        public int rId { get; set; }
        public decimal total { get; set; }
        public decimal companyIncome { get; set; }
        public decimal restaurantIncome { get; set; }

        public DateTime transactionDate { get; set; }

        public transaction transaction { get; set; }


    }
}