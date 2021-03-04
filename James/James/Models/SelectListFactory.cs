using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Mvc;
using System.Data.SqlClient;
using MoreLinq;
using System.Data;
using FormFactory;
using System.Text;

namespace James.Models
{
    public class SelectListFactory
    {
        private dbjEntities1 db = new dbjEntities1();


        
        

        public List<SelectListItem> ProvinceList()
        {
            var CAProvince = new List<SelectListItem>();
            //string sSQL = "select distinct province from CanadaCities ";
            var CACity = new List<SelectListItem>();

            CAProvince.Add(new SelectListItem { Text = "select province", Disabled = true, Selected = true });
            CAProvince.AddRange(db.CanadaPV.Select(x =>
            new SelectListItem
            {
                Text = x.province,
                Value = x.pvId.ToString(),
                
            }

            ).DistinctBy(x=>x.Text).ToList());



         

            return CAProvince;
            
        }

       
        public List<SelectListItem> CityList(int pvId)
        {



            var CACity = new List<SelectListItem>();
            //string sSQL = "select distinct province from CanadaCities ";



            CACity.Add(new SelectListItem { Text = "select city", Disabled = true, Selected = true });
            CACity.AddRange(db.CanadaCities.Where(x=>x.pvId==pvId).Select
                
                (x =>
            new SelectListItem
            {
                Text = x.city,
                Value = x.pvId.ToString()
            }

            ).ToList());
            return CACity;

        }


        /// <summary>
        /// Gets the products.
        /// </summary>
        /// <param name="orderID">The order ID.</param>
        /// <returns></returns>
       


    }
}