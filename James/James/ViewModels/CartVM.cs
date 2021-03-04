using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using James.Models;

namespace James.ViewModels
{
    public class CartVM
    {
        public string productName { get; set; }
        public decimal productPrice { get; set; }
        public string productImg { get; set; }
        public string productStyle { get; set; }
        public string orderAdd { get; set; }



        //list info

        [Key]

        public int oId { get; set; }
        public int cId { get; set; }
        public int pId { get; set; }
        public int orderQty { get; set; }
        

        public List<Product> productList { get; set; }

        public List<orderDetail> orderDetails { get; set; }
        public decimal totalPrice { get; set; }
        public int status { get; set; }
    }
    
    public class orderDetail
    {
        public int oId { get; set; }
        public int cId { get; set; }
        public int pId { get; set; }
        public OrderList orders { get; set; }
        public string productName { get; set; }
        public decimal productPrice { get; set; }
        public string productImg { get; set; }
        public string productStyle { get; set; }
        public string orderAdd { get; set; }
        public int orderQty { get; set; }
        public List<Product> prod { get; set; }

        public decimal TP { get; set; }
    }
}