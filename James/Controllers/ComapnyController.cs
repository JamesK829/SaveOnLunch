using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using James.Models;
using James.ViewModels;

namespace James.Controllers
{
    public class ComapnyController : Controller
    {
        // GET: Comapny

        dbjEntities1 db = new dbjEntities1();

        public ActionResult CompanyTransactions()
        {


            var tList = db.transaction.OrderBy(x => x.tId);
            //var trans = new List<trans>();
            var T = (from t in db.transaction
                     join c in db.Customer on t.cId equals c.cId
                     join o in db.OrderList on t.cId equals o.cId


                     select new trans
                     {
                         cId = (int)t.cId,
                         customerName = c.cFName + c.cLName,
                         customerEmail = c.cEmail,
                         companyIncome = (decimal)t.companyIncome,
                         transactionDate = (DateTime)t.tDate




                     }).ToList();
            ViewBag.TotalIncome = T.Sum(x => x.companyIncome).ToString("0.##");

            var vm = new vTransactionVM
            {
                trans = (List<trans>)T,
                total = T.Sum(x => x.companyIncome)


            };

            return View();

        }
    }
}