using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using James.Models;
using System.Web.Mvc;
using MoreLinq;


namespace James.Models
{
    public class SelectionFactory
    {
        dbjEntities1 db = new dbjEntities1();

        public List<SelectListItem> SelectStyle()
        {
            var StyleOption = new List<SelectListItem>();
            StyleOption.Add(new SelectListItem
            {
                Text = "select style",
                Disabled = true,
                Selected = true

            });
            StyleOption.AddRange(db.Style.Select(x => new SelectListItem
            {
                Text = x.style,
                Value = x.sId.ToString()

            }).ToList());

            return StyleOption;
        }

        public List<SelectListItem> SelectComapny()
        {
            var CompanyOption = new List<SelectListItem>();
            CompanyOption.Add(new SelectListItem
            {
                Text = "select company name",
                Disabled = true,
                Selected = true

            });
            CompanyOption.AddRange(db.Company.Select(x => new SelectListItem
            {
                Text = x.CPName,  
                Value = x.CPId.ToString()
            }).DistinctBy(x=>x.Text).ToList());

            return CompanyOption;
        }
        public List<SelectListItem> SelectComapnyBranch()
        {
            var CompanyOption = new List<SelectListItem>();
            CompanyOption.Add(new SelectListItem
            {
                Text = "select company branch",
                Disabled = true,
                Selected = true

            });
            CompanyOption.AddRange(db.Company.Select(x => new SelectListItem
            {
                Text = x.CPBranch,
                Value = x.CPName.ToString()
            }).DistinctBy(x => x.Text).ToList());

            return CompanyOption;
        }
        public List<SelectListItem> SelectProvince()
        {
            
            var ProvinceOption = new List<SelectListItem>();
            ProvinceOption.Add(new SelectListItem
            {
                Text = "select company branch",
                Disabled = true,
                Selected = true

            });
            ProvinceOption.AddRange(db.CanadaPV.Select(x => new SelectListItem
            {
                Text = x.province,
                Value = x.pvId.ToString()
            }).ToList());

            return ProvinceOption;
        }
        public List<SelectListItem> SelectCity()
        {
            var ProvinceOption = new List<SelectListItem>();
            ProvinceOption.Add(new SelectListItem
            {
                Text = "select company branch",
                Disabled = true,
                Selected = true

            });
            ProvinceOption.AddRange(db.CanadaCities.Select(x => new SelectListItem
            {
                Text = x.city,
                Value = x.cityId.ToString()
            }).ToList());

            return ProvinceOption;
        }

        //public ActionResult SelectRegion(int pvId)
        //{
        //    if (pvId == -1)
        //    {
        //        pvId = db.CanadaPV.First().pvId;

        //    }
        //    //var SelectPV = db.CanadaPV.GroupBy(c => c.pvId).Select(g => g.FirstOrDefault()).ToList();
        //    //int Province = SelectPV.FirstOrDefault().pvId;
        //    //var SelectCity = db.CanadaCities.Where(c => c.pvId == Province).ToList();

        //    List<SelectListItem> PV = new List<SelectListItem>();
        //    PV.AddRange(db.CanadaPV.Select(x => new SelectListItem
        //    {
        //        Text = x.province,
        //        Value = x.pvId.ToString()
        //    }).ToList());
        //    string Province = PV.FirstOrDefault().Value;
        //    List<SelectListItem> CT = new List<SelectListItem>();
        //    CT.AddRange(db.CanadaCities.Where(c=>c.pvId.ToString()==Province).Select(x => new SelectListItem
        //    {
        //        Text = x.city,
        //        Value = x.cityId.ToString()
        //    }).ToList());

        //    return View();

        //}

    }
}