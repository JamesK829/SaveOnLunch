using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using James.Models;
using PagedList;
using System.Web.Mvc;

namespace James.ViewModels
{
    public class vProductListVM
    {
        public ProductSearchModel searchParameter { get; set; }

        public int pageIndex { get; set; }

        public IPagedList<Product> products { get; set; }


        public SelectList styleItems { get; set; }

        public int ordernum { get; set; }

    }
    public class ProductSearchModel
    {
        public string style { get; set; }
        public string productName { get; set; }
    }
}