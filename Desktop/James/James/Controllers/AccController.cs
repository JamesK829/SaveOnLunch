using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using James.Models;
using James.ViewModels;
using System.Data.SqlClient;
using James.Models.Common;
using System.Data;
using James.ViewModels.Account;

namespace James.Controllers
{
    public class AccController : Controller
    {
        dbjEntities1 db = new dbjEntities1();
        
     
        private IEnumerable<Company> companies
        {
            get { return (IEnumerable<Company>)db.Company.OrderBy(c => c.CPId); }
        }
        public ActionResult Index()
        {
            var product = db.Product.ToList();
            if (Session[CDictionary.SK_PAGE_BEFORE_LOGIN] == null)
                return PartialView("Index",  product);
            return View("Index", "_LayoutPage", product);
        }


        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(vLoginVM LoginVM)
        {

            if (ModelState.IsValid)
            {
                var member = db.Customer.FirstOrDefault(m => m.cEmail == LoginVM.Account && m.cPwd == LoginVM.Password);
                
                if (member != null)
                {
                    Session[CDictionary.SK_LOGINED_USER_ID] = member.cId;
                    Session[CDictionary.SK_MEMBER_AUTHORITY] = member.cAuthority;
                    var OList = db.OrderList.Where(o => o.cId == member.cId);

                    var orders = new List<orderDetail>();

                    foreach (var item in OList)
                    {
                        orders.Add(new orderDetail
                        {
                            oId = item.oId,
                            // pId = item.pId,
                            orderQty = (int)item.oQty,
                            productPrice = (decimal)item.Product.pPrice,
                            //    productName = item.Product.pName,
                            TP = (decimal)((decimal)item.oQty * item.Product.pPrice),



                        });

                    }
                    // ViewBag.total = (orders.Sum(x => x.TP)).ToString("#.##");
                    //ViewBag.tax = (orders.Sum(x => x.TP) * (decimal)0.12).ToString("#.##");
                  //  Session[CDictionary.TK_Cart_TOTALPRICE] = (orders.Sum(x => x.TP) * (decimal)1.12).ToString("#.##");
                    //);


                    if (Session[CDictionary.SK_PAGE_BEFORE_LOGIN] != null)
                    {
                        
                        //return last page
                        var page = Session[CDictionary.SK_PAGE_BEFORE_LOGIN] as page;
                        return RedirectToAction(page.action, page.controller, page.urlArgs);

                    }
                    else
                    {
                        Session["Welcome"] = member.cFName + "Welcome";
                        Session[CDictionary.SK_PAGE_BEFORE_LOGIN] = member;
                        Session[CDictionary.TK_Cart_TOTALPRICE] = (orders.Sum(x => x.TP) * (decimal)1.12).ToString("#.##");

                        return RedirectToAction("CCenterList");
                    }

                }
                else
                {
                    ModelState.AddModelError("Account", "Error Account Fail To LogIn");
                    ModelState.AddModelError("Password", "Error Password Fail To LogIn");
                }
                
                return View(LoginVM);

            }
            //    if (member == null)
            //    {
            //        ViewBag.Message = "Wrong Password/Account";
            //        return View();
            //    }
            //}
            //Session["Welcome"] = member.cFName + "Welcome";
            //Session["Member"] = member;

            return RedirectToAction("Index");

        }
       
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        public ActionResult CompanyCreate()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CompanyCreate(vCompanyVM vm)
        {

            var userId = (int)Session[CDictionary.SK_LOGINED_USER_ID];

            var currentCP = db.Company.Where(c => c.CPName == vm.companyName && c.CPBranch == vm.companyBranch).FirstOrDefault();
            if (currentCP == null)
            {
                db.Company.Add(new Models.Company
                {
                    CPName=vm.companyName,
                    CPBranch=vm.companyBranch,
                    CPAdd=vm.companyAdd,
                    CPPhone=vm.companyPhone

                  
                });
                db.SaveChanges();
            }
            else
            {
                
            }



            return View(vm);
        }

        public ActionResult Register()
        {

          ViewBag.CPId = new SelectList(db.Company, "CPId", "CPName");


            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(vRegisterVM vm)
        {




            if (ModelState.IsValid == false) return View();

            var customer = db.Customer.FirstOrDefault(m => m.cEmail == vm.Account);
            if (ModelState.IsValid)
            {
                if (customer != null)
                {
                    ViewBag.Message = "This Account Is Already Exist";
                    return View();
                }
                if (customer == null)
                {
                     Customer member = new Customer
                      {
                       cEmail=vm.Account,
                       cPwd=vm.Password
                      };
                        db.Customer.Add(member);
                        db.SaveChanges();
                        return RedirectToAction("Login");
                }

            }

            ViewBag.Message = "This Account Is Already Exist";


            return View();
        }



        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("login");
        }


        [Authority(authority=CAauthority.normalCustomer)]
        public ActionResult CCenterList()
        {
            int userId = (int)Session[CDictionary.SK_LOGINED_USER_ID];
            var w =(
                  from o in db.OrderList
                  where o.cId == userId
                  join p in db.Product on o.pId equals p.pId
                  select new CartVM
                  {
                      totalPrice = ((decimal)(o.oQty * p.pPrice))
                  });
            if (db.OrderList.FirstOrDefault(o=>o.cId==userId)!= null)
            {
                Session[CDictionary.TK_Cart_TOTALPRICE] = w.Sum(p => p.totalPrice).ToString("0.##");
            }
            else
            {
                Session[CDictionary.TK_Cart_TOTALPRICE] = 0;
            }
            

            var user = (from c in db.Customer
                        join a in db.Admin on c.cId equals a.cId into AX
                       join cp in db.Company on c.CPId equals cp.CPId  into AY
                        from x in AX.DefaultIfEmpty()  
                        from y in AY.DefaultIfEmpty()
                       
                       
                        where c.cId == userId
                       
                        select new vMemberCenterVM
                        {
                            firstName=c.cFName,
                            lastName=c.cLName,
                            email=c.cEmail,
                            phoneNumber=c.cPhone,
                            avatar=c.cAvatar,
                            point=c.cPoint,
                            birthday=c.cBD,
                            companyId = c.CPId,
                            companyAdd=y.CPAdd,
                            companyBranch=y.CPBranch,
                            companyName=y.CPName,
                            aId=x.aId,
                            adminDepartment=x.aDepartment,
                            tp= w.Sum(p=>p.totalPrice).ToString(),
                           // tax=( w.Sum(p => p.totalPrice)*(decimal)0.12).ToString("0.##")
                        }).FirstOrDefault();

          
            // Session[CDictionary.TK_Cart_TOTALPRICE] = x.Sum(a=>a.totalPrice).ToString();
           // Session[CDictionary.TK_Cart_TOTALPRICE] = w.Select(a => a.totalPrice).ToString();

            if (Session[CDictionary.SK_LOGINED_USER_ID]!=null && Session[CDictionary.SK_LOGINED_ADMIN_ID ]!= null)
            {
                Session[CDictionary.SK_LOGINED_ADMIN_ID] = user.aId;
              


            }

            return View(user);
        }

        public ActionResult CCenter()
        {

            int userId = (int)Session[CDictionary.SK_LOGINED_USER_ID];

            var user = db.Customer.FirstOrDefault(c => c.cId == userId);
            var ListCP = (new SelectionFactory()).SelectComapny();
            var ListCPBranch = (new SelectionFactory()).SelectComapnyBranch();



            var vm =  new vMemberCenterVM
                      {
                          firstName = user.cFName,
                          lastName = user.cLName,
                          email = user.cEmail,
                          phoneNumber = user.cPhone,
                          avatar = user.cAvatar,
                          point = user.cPoint,
                          birthday = user.cBD,
                          
                          companyId = user.CPId,
                          companylist=ListCP,
                          companybranchlist=ListCPBranch
  
                      } ;


            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CCenter(vMemberCenterVM vm)
        {

            if (ModelState.IsValid)
            {
                
               vm.cId = (int)Session[CDictionary.SK_LOGINED_USER_ID];


                var user = db.Customer.FirstOrDefault(m=>m.cId==vm.cId);
                if (vm.image != null)
                {
                    var 新菜品照片地址 = (new 共用方法()).照片更新(vm.image, Server.MapPath("~/"), CDictionary.picAdd_member, user.cAvatar);
                    user.cAvatar = 新菜品照片地址;
                }

                user.cFName = vm.firstName;
                user.cLName = vm.lastName;
                user.cPhone = vm.phoneNumber;
                user.cBD = vm.birthday;
                

                db.SaveChanges();

                return RedirectToAction("CCenterList","Acc");

            }

            var ListCP = (new SelectionFactory()).SelectComapny();
            ListCP.First(x => x.Value == vm.CPId.ToString());
            var ListCPBranch = (new SelectionFactory()).SelectComapnyBranch();
            ListCPBranch.First(x => x.Value == vm.companyName);
            return View(vm);

            

            
        }
      //public ActionResult CreateCompany()
      //  {
      //      var provinceList = (new SelectionFactory()).SelectProvince();
            

      //      var vm = new vCompanyVM
      //      {

      //          selectPV = provinceList,
      //          pvId= provinceList.GetHashCode()
      //      };
      //      return View(vm);

      //  }
      //  [HttpPost]
      //  public ActionResult CreateCompany(vCompanyVM vm)
      //  {
      //      if (ModelState.IsValid)
      //      {



      //          db.Company.Add(new Models.Company { 
      //          pvId=vm.pvId,
      //          cityId=vm.cityId,
      //          CPName=vm.companyName,
      //          CPBranch=vm.companyBranch,
      //          CPPhone=vm.companyPhone,
      //          CPAdd=vm.companyAdd
                
      //          });
      //          db.SaveChanges();
      //          return RedirectToAction("CCenter", "Acc");

                
      //      }
      //      var provinceList = (new SelectionFactory()).SelectProvince();
      //      provinceList.First(a => a.Value == vm.pvId.ToString()).Selected=true;

      //      vm.selectPV = provinceList;
      //      db.SaveChanges();

      //      return View(vm);

      //  }
        public ActionResult GetCityByProvince(string province)
        {
            return Json(new SelectList(db.CanadaCities.Where(c => c.province == province), "cityId", "city"));
        }

        //  [Authority(authority =CAauthority.restaurant)]
        public ActionResult RestaurantCreate()
        {


          
            //var SelectPV = db.CanadaPV.GroupBy(c => c.pvId).Select(g => g.FirstOrDefault()).ToList();
            //int Province = SelectPV.FirstOrDefault().pvId;
            //var SelectCity = db.CanadaCities.Where(c => c.pvId == Province).ToList();

            List<SelectListItem> PV = new List<SelectListItem>();
            PV.AddRange(db.CanadaPV.Select(x => new SelectListItem
            {
                Text = x.province,
                Value = x.pvId.ToString()
            }).ToList());
            string Province = PV.FirstOrDefault().Value;
            List<SelectListItem> CT = new List<SelectListItem>();
            CT.AddRange(db.CanadaCities.Where(c => c.pvId.ToString() == Province).Select(x => new SelectListItem
            {
                Text = x.city,
                Value = x.cityId.ToString()
            }).ToList());


            var vm = new vRestaurantVM
            {
                cId = (int)Session[CDictionary.SK_LOGINED_USER_ID],
                province = PV,
                city = CT


            };

            return View(vm);


        }
        [HttpPost]
       
        public ActionResult RestaurantCreate(vRestaurantVM vm)
        {
            if (ModelState.IsValid)
            {
                var user = db.Customer.FirstOrDefault(c => c.cId == vm.cId);

                //get authority
                user.cAuthority = CAauthority.restaurant.GetHashCode();

                db.Restaurant.Add(new Restaurant
                {
                    cId=vm.cId,
                    province=vm.RProvince,
                    city=vm.RCity,

                });

                db.SaveChanges();
                return RedirectToAction("ProductCreate","Product");
            }
            else
            {
                List<SelectListItem> PV = new List<SelectListItem>();
                PV.AddRange(db.CanadaPV.Select(x => new SelectListItem
                {
                    Text = x.province,
                    Value = x.pvId.ToString()
                }).ToList());
                string Province = PV.FirstOrDefault().Value;
                List<SelectListItem> CT = new List<SelectListItem>();
                CT.AddRange(db.CanadaCities.Where(c => c.pvId.ToString() == Province).Select(x => new SelectListItem
                {
                    Text = x.city,
                    Value = x.cityId.ToString()
                }).ToList());

                PV.First(x => x.Value == vm.RProvince);
                vm.province = PV;
                CT.First(x => x.Value == vm.RCity);
                vm.city = CT;


            }
            return View(vm);

        }
       
       

        public ActionResult AddCompany(string pv)
        {
            var SelectPV = (new SelectListFactory()).ProvinceList();

            var vm = new vCompanyVM
            {
                
                selectPV = SelectPV,

            };

           

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCompany(vCompanyVM vm, string province)
        {

           

            if (ModelState.IsValid == false) return View();

           
            if (ModelState.IsValid)
            {

              

               
                    

                db.Company.Add(new Models.Company
                {
                    CPId=vm.CPId,
                    CPName=vm.companyName,
                    CPBranch=vm.companyBranch,
                    CPAdd=vm.companyAdd,
                    CPPhone=vm.companyPhone


                });
                var SelectPV = (new SelectListFactory()).ProvinceList();
                vm.selectPV = SelectPV;

              

                db.SaveChanges();
                    return RedirectToAction("CCenter");
              //  }

            }

            ViewBag.Message = "This Company Is Already In List";

            return View();
        }

      
        //public Company GetById(int cpid)
        //{
        //    List<SqlParameter> paras = new List<SqlParameter>();
        //    paras.Add(new SqlParameter("CPId", (object)cpid));
        //    List<Company> list = getBySql("select * from Company where cpid=@CPId", paras);
        //    if (list.Count == 0) return null;
        //    return list[0];

        //}
        //private List<Company> getBySql(string sql, List<SqlParameter> paras)
        //{


        //    SqlConnection con = new SqlConnection();
        //    con.ConnectionString = @"Data Source=.;Initial Catalog=dbDemo;Integrated Security=True";
        //    con.Open();


        //    SqlCommand cmd = new SqlCommand();
        //    cmd.Connection = con;
        //    cmd.CommandText = sql;
        //    if (paras != null)
        //    {
        //        foreach (SqlParameter p in paras)
        //            cmd.Parameters.Add(p);
        //    }

        //    SqlDataReader reader = cmd.ExecuteReader();
        //    List<Company> list = new List<Company>();


        //    // ViewBag.aa = "anonymous.jpg";


        //    while (reader.Read())
        //    {
        //        Company x = new Company();
        //        x.CPId = (int)reader["CPId"];
        //        x.CPName = reader["CPName"].ToString();
        //        x.CPPhone = (int)reader["CPPhone"];
        //        x.CPAdd = reader["CPAdd"].ToString();



        //        list.Add(x);
        //    }

        //con.Close();
        //return list;
        //}

        //public ActionResult AddCart(int pId, string CPName)
        //{


        //    int cId = (Session["Member"] as Customer).cId;
        //var CPId = (Session["Member"] as Customer).CPId;





        //SqlConnection con = new SqlConnection();
        //con.ConnectionString = @"Data Source=.;Initial Catalog=dbSOL;Integrated Security=True";
        //con.Open();
        //SqlCommand cmd = new SqlCommand("select CPId,CPName from Company where CPId = @CPId", con);

        //SqlDataReader reader = cmd.ExecuteReader();
        //while (reader.Read())
        //{
        //    string id = reader[0] as string;
        //    string name = reader[1] as string;
        //    list.Add(new SelectListItem() { Text = name, Value = id });
        //}
        //con.Close();

        //SqlDataAdapter sda = new SqlDataAdapter(cmd);
        //DataTable dt = new DataTable();
        //sda.Fill(dt);
        //ViewBag.ddl = new SelectList(dt.AsDataView(), "CPId", "CPName");

        //  SelectList SL= new SelectList(dt.AsDataView(), "CPId", "CPName");


        //var customer = db.Customer.FirstOrDefault(m => m.cId == cId);
        //var companies = db.Company.Where(m => m.CPId == CPId)
        //    .OrderBy(m => m.CPName)
        //    .ToList();
        //var clist = (from p in companies
        //             select new SelectListItem { Value = CPId.ToString(), Text = CPName }).ToList<SelectListItem>();






        //    var currentCart = db.OrderList
        //        .Where(m => m.pId == pId && m.cId == cId)
        //        .FirstOrDefault();
        //    if (currentCart == null)
        //    {

        //        var product = db.Product.FirstOrDefault(m => m.pId == pId);
        //        OrderList orders = new OrderList();
        //        orders.cId = cId;
        //        orders.pId = product.pId;
        //        orders.oDate = DateTime.Now;


        //        orders.oQty = 1;
        //        db.OrderList.Add(orders);

        //    }
        //    else
        //    {
        //        currentCart.oQty += 1;
        //    }
        //    db.SaveChanges();

        //    return RedirectToAction("Cart");
        //}







    }
}