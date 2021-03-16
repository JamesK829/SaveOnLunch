using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace James.ViewModels
{
    public class vCompanyVM
    {
        public int CPId {get;set;}
        public string companyName { get; set; }
        public string companyBranch { get; set; }
        public string companyProvince { get; set; }
        public string companyCity { get; set; }
        public string companyAdd { get; set; }
        public string companyPhone { get; set; }

        public int pvId { get; set; }
        public int cityId { get; set; }
        public List<SelectListItem> selectPV { get; set; }

        public string selectedPV { get; set; }

        public List<SelectListItem> CACity { get; set; }

     

    }
}