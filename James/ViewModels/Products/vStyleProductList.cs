using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using James.Models;

namespace James.ViewModels.Products
{
    public class vStyleProductList
    {
        public Style style { get; set; }
        public List<Product> products { get; set; }
    }
}