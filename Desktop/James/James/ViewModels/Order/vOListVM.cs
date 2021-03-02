using James.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace James.ViewModels
{




    public class vOListVM
    {
      

       public int cId { get; set; }

        public int oId { get; set; }
        public decimal tax { get; set; }

        public DateTime serverDate { get; set; }

        public transaction transaction { get; set; }

        public List<olist> olist {get;set;}

        public decimal priceWithTax { get; set; }
        public decimal cartPrice { get; set; }

    }
    public class olist
    {
        public int oId { get; set; }
        public int cId { get; set; }

        public string productName { get; set; }
        public int orderQty { get; set; }
        public decimal productPrice { get; set; }
 public decimal totalPrice { get; set; }
    }
}