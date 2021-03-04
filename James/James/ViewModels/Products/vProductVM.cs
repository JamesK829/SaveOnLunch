using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using James.Models;

namespace James.ViewModels
{
    public class vProductVM
    {

        public vProductVM()
        {

            this.productImg = James.Models.Common.CDictionary.DefaultImg;
        }
        [Key]
        public int pId { get; set; }
        public int cId { get; set; }
        public string productName { get; set; }
        public decimal productPrice { get; set; }
        public string productImg { get; set; }
        public int rId { get; set; }
        public System.DateTime offerdate{ get; set; }
        public string ArrGetNewTime { get; set; }
        public string ArrGetDeletedTime { get; set; }
        public List<string> ArrEnable { get; set; }
        public List<string> ArrUnable { get; set; }
        [Display(Name = "Style")]
        public int sId { get; set; }
        public List<SelectListItem> Style { get; set; }
        public string productstyle { get; set; }

        public int ordernum { get; set; }
        public int oId { get; set; }
        public string productContent { get; set; }

        public HttpPostedFileBase image { get; set; }

      

        public int pageIndex { get; set; }
    }
   



}
