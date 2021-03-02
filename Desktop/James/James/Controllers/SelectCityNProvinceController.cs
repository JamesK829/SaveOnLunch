using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using James.Models;
using System.Collections;
using Microsoft.Ajax.Utilities;
using MoreLinq;

namespace James.Controllers
{
    public class SelectCityNProvinceController : Controller
    {

        dbjEntities1 db = new dbjEntities1();

        // GET: SelectCityNProvince
        //public ActionResult GetProvinces()
        //{ 
        //    var entities = locationService.GetProvinceList(); 
        //    return Json(entities, JsonRequestBehavior.AllowGet); 
        //}
        //public ActionResult GetCities(String provinceCode)
        //{ 
        //    var entities = locationService.GetCityList(new CityQuery() { ProvinceCode = provinceCode }); 
        //    return Json(entities, JsonRequestBehavior.AllowGet); 
        //}
        //public ActionResult GetCounties(String cityCode) 
        //{
        //    var entities = locationService.GetCountyList(new CountyQuery() { CityCode = cityCode }); 
        //    return Json(entities, JsonRequestBehavior.AllowGet); 
        //}
        /// <summary> /// 測試頁Action /// </summary> /// <returns></returns> 
        /// public ActionResult SelectControlTest() { return View(); }
    }
}