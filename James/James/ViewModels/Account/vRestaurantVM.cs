using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace James.ViewModels
{
    public class vRestaurantVM
    {
        [Key]
        public int rId { get; set; }
        public int cId { get; set; }
        public string RestaurantName { get; set; }
        public string RProvince { get; set; }
        public string RCity { get; set; }
        public string RAddress { get; set; }

        public List<SelectListItem> province { get; set; }
        public List<SelectListItem> city { get; set; }
    }
}