using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using James.Models.Common;
using James.Models;

namespace James.ViewModels.Account
{
    public class vMemberCenterVM
    {
  
      public int cId { get; set; }
        public int CPId { get; set; }
       
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string phoneNumber { get; set; }
        public decimal? point { get; set; }
        public string birthday { get; set; }
        public string avatar { get; set; }
        public int? companyId { get; set; }

        public string companyName { get; set; }
        public string companyAdd { get; set; }
        public string companyBranch { get; set; }
        public string companyProvince { get; set; }
        public string companyCity { get; set; }
        public string tp { get; set; }
        public string tax { get; set; }
        public List<SelectListItem> companylist { get; set; }

        public List<SelectListItem> companybranchlist { get; set; }
        //Admin

        public int? aId { get; set; }
        public string adminDepartment { get; set; }

        public int cityId { get; set; }
        public List<SelectListItem> selectPV { get; set; }
        public HttpPostedFileBase image { get; set; }

    }
}