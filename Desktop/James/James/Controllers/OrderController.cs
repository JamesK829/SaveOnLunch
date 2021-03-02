using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using James.Models;
using System.Data.SqlClient;
using System.Data.Entity;
using James.Models.Common;
using James.ViewModels;


namespace James.Controllers
{

    public class OrderController : Controller
    {

        dbjEntities1 db = new dbjEntities1();

        //private string strCart = "Cart";

   
        public ActionResult BuyPoint()
        {

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BuyPoint(vStorePointVM vStorePoint)
        {

            if (ModelState.IsValid)
            {
                int userId = (int)Session[CDictionary.SK_LOGINED_USER_ID];
                var perchaseP = new StorePoint
                {
                  cId=userId,
                  point=vStorePoint.point
                };
                db.StorePoint.Add(perchaseP);
                var user = db.Customer.FirstOrDefault(x => x.cId == userId);
                user.cPoint += vStorePoint.point;
                db.SaveChanges();
                return RedirectToAction("CCenterList", "Acc");

            }


            return View(vStorePoint);
        }


        // GET: Order
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Cart()
        {
            int cId = (int)Session[CDictionary.SK_LOGINED_USER_ID] ;
            var orderList = db.OrderList.Where
                (m => m.cId == cId)
                .ToList();


            return View("Cart", "_LayoutMember", orderList);
        }
      
   
        public ActionResult Purchase(CartVM vm)
        {

            int userId = (int)Session[CDictionary.SK_LOGINED_USER_ID];

            var currentCart = db.OrderList
                .Where(m => m.oId == vm.oId && m.cId == userId)
                .FirstOrDefault();

            if (currentCart == null)
            {

                //var product = db.Product.FirstOrDefault(m => m.pId == vm.pId);

                //db.OrderList.Add(new OrderList
                //{
                //    pId = product.pId,
                //    oDate = System.DateTime.Now,
                //    oQty = vm.orderQty,
                //    cId = cId


                //});
           


              
              
              

           
                currentCart.oQty += 1;
                db.SaveChanges();

return RedirectToAction("OrderList");
            }



            //  Session[CDictionary.TK_Cart_Qty] = 0;

            return View(vm);

            

        }

        public ActionResult OrderList()
        {
            
              int userId = (int)Session[CDictionary.SK_LOGINED_USER_ID];

            var OList = db.OrderList.Where(o => o.cId == userId);

            
            var orders = new List<orderDetail>();
            
            foreach(var item in OList)
            {
                orders.Add(new orderDetail
                {
                    oId=item.oId,
                    pId = item.pId,
                    orderQty = (int)item.oQty,
                    productPrice = (decimal)item.Product.pPrice,
                    productName = item.Product.pName,
                    TP = (decimal)((decimal)item.oQty * item.Product.pPrice),



                }) ;
           
            }
            ViewBag.total = (orders.Sum(x => x.TP)).ToString("#.##");
            ViewBag.tax = (orders.Sum(x => x.TP) * (decimal)0.12).ToString("#.##");
           Session[CDictionary.TK_Cart_TOTALPRICE] = (orders.Sum(x => x.TP)*(decimal)1.12).ToString("#.##");

            var vm = new CartVM
            {
                orderDetails = orders,
                cId = userId,
                

        };
           

            return View(vm);



        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult OrderList(CartVM vm)
        {
            int userId = (int)Session[CDictionary.SK_LOGINED_USER_ID];

            var currentCart = db.OrderList
                .Where(m=>m.oId==vm.oId&&m.cId==vm.cId).ToList();
               

            if (currentCart!=null)
            {

             
              foreach(var item in currentCart)
                {
                 item.oQty += vm.orderQty;
                }

                   
                    db.SaveChanges();
                

                    
              
                    
                   
                
                
                
                
            return RedirectToAction("OrderList", "Order");
        }
            //currentCart.oQty += vm.orderQty;
            //db.SaveChanges();
            return RedirectToAction("ProductList", "Product");
           //  return RedirectToAction("OrderList", "Order");

        }
        public ActionResult ConfirmCart()
        {
            var userId = (int)Session[CDictionary.SK_LOGINED_USER_ID];

            var currentCart = db.OrderList.Where(o => o.cId == userId);
            var orderList = new List<olist>();
            foreach(var item in currentCart)
            {
                orderList.Add(new olist
                {
                    cId=userId,
                    oId = item.oId,
                    productName = item.Product.pName,
                    productPrice = (decimal)item.Product.pPrice,
                    orderQty = item.oQty,
                    totalPrice=(decimal)item.Product.pPrice*item.oQty


                });


            }
            var vm = new vOListVM
            {
                
                olist = orderList,
                cartPrice=(decimal)orderList.Sum(x=>x.totalPrice),
                tax = orderList.Sum(x => x.totalPrice) * (decimal)0.12,
                priceWithTax = orderList.Sum(x => x.totalPrice) * (decimal)1.12


            };
            Session[CDictionary.TK_Cart_TOTALPRICE] = vm.priceWithTax;
            return View(vm);

        }
        [HttpPost]
        public ActionResult ConfirmCart(vOListVM vm)
        {
            if (ModelState.IsValid)
            {
                vm.cId = (int)Session[CDictionary.SK_LOGINED_USER_ID];
                vm.priceWithTax = (decimal)Session[CDictionary.TK_Cart_TOTALPRICE];

                var user = db.Customer.FirstOrDefault(c => c.cId == vm.cId);

                var currentCart = db.OrderList
                .Where(m => m.cId == vm.cId);

                foreach(var item in currentCart)
                {
                    item.oRDate = vm.serverDate;
                    item.oStatus = orderStatus.afterCheckOut.GetHashCode();

                }

                user.cPoint -= vm.priceWithTax;

                transaction ts = new transaction();
                ts.cId = vm.cId;
                ts.companyIncome = vm.priceWithTax * (decimal)0.9;
                ts.tDate = DateTime.Now;
                db.transaction.Add(ts);
                db.SaveChanges();
                return RedirectToAction("CCenterList","Acc");
            }

            return View(vm);
        }
        public ActionResult transaction(vTransactionVM vm)
        {

            vm.total = (decimal)Session[CDictionary.TK_Cart_TOTALPRICE];

            var userId = (int)Session[CDictionary.SK_LOGINED_USER_ID];


            transaction ts = new transaction();
            ts.cId = userId;
            ts.companyIncome = vm.total * (decimal)0.9;
            ts.tDate = DateTime.Now;
            db.transaction.Add(ts);

            return View("ProductList","Product");
        }
        public ActionResult OList()
        {
            int userId = (int)Session[CDictionary.SK_LOGINED_USER_ID];
            var OList = db.OrderList.Where(o => o.cId == userId);


            var orders = new List<orderDetail>();

            foreach (var item in OList)
            {
                orders.Add(new orderDetail
                {
                    cId=userId,
                    oId = item.oId,
                    pId = item.pId,
                    orderQty = (int)item.oQty,
                    productPrice = (decimal)item.Product.pPrice,
                    productName = item.Product.pName,
                    TP = (decimal)((decimal)item.oQty * item.Product.pPrice),



                });

            }
            ViewBag.total = (orders.Sum(x => x.TP)).ToString("#.##");
            ViewBag.tax = (orders.Sum(x => x.TP) * (decimal)0.12).ToString("#.##");
            Session[CDictionary.TK_Cart_TOTALPRICE] = (orders.Sum(x => x.TP) * (decimal)1.12).ToString("#.##");




            var vm = new CartVM
            {
                orderDetails = orders,
                cId = userId,


            };


            return View(vm);
            //int userId = (int)Session[CDictionary.SK_LOGINED_USER_ID];

            ////var user =db.Customer.FirstOrDefault(c => c.cId == cId);
            ////Session[CDictionary.SK_LOGINED_USER_ID] = user.cId;

            //var vm = (from o in db.OrderList
            //          join p in db.Product on o.pId equals p.pId into op
            //          from a in op
            //          where o.cId == userId
            //          select new orderDetail
            //          {
            //              cId=userId
            //              oId = o.oId,
            //              productName = a.pName,
            //              productPrice = (decimal)a.pPrice,
            //              orderQty = (int)o.oQty,
            //              TP = ((decimal)a.pPrice) * (int)o.oQty


            //          }

            //).ToList();


            //ViewBag.total = (vm.Sum(x => x.TP)).ToString("#.##");
            //ViewBag.tax = (vm.Sum(x => x.TP) * (decimal)0.12).ToString("#.##");
            //Session[CDictionary.TK_Cart_TOTALPRICE] = (vm.Sum(x => x.TP) * (decimal)1.12).ToString("#.##");



            //return View(vm);



        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult OList(orderDetail vm)
        {
            int userId = (int)Session[CDictionary.SK_LOGINED_USER_ID];

            var currentCart = db.OrderList
                .Where(m => m.oId == vm.oId);


            if (currentCart != null)
            {


                foreach(var item in currentCart)
                item.oQty += vm.orderQty;





                db.SaveChanges();

                return RedirectToAction("OList", "Order");
            }
            return View(currentCart);
        }
        public ActionResult UpdateCart(CartVM vm)
        {


            int userId = (int)Session[CDictionary.SK_LOGINED_USER_ID];

            var currentCart = db.OrderList
                .Where(m => m.oId == vm.oId && m.cId == userId);
               

            if (currentCart != null)
            {
                foreach(var item in currentCart)
                {
                    item.oQty = vm.orderQty;
                }
           
            
            db.SaveChanges();
             return RedirectToAction("OrderList");
            }


            return View(currentCart);
           
        }


        public ActionResult AddCart(CartVM vm)
        {


            int userId = (int)Session[CDictionary.SK_LOGINED_USER_ID];
            
            var currentCart = db.OrderList
                .Where(m => m.pId == vm.pId && m.cId == userId)
                .FirstOrDefault();

            if (currentCart == null)
            {

                var product = db.Product.FirstOrDefault(m => m.pId == vm.pId);

                //db.OrderList.Add(new OrderList
                //{
                //    pId = product.pId,
                //    oDate = System.DateTime.Now,
                //    oQty = vm.orderQty,
                //    cId = cId


                //});
                OrderList orders = new OrderList();
                orders.cId = userId;
                orders.pId = product.pId;
                orders.oDate = DateTime.Now;


                orders.oQty = 1;
                db.OrderList.Add(orders);

            }
            else
            {
                currentCart.oQty += 1;
            }
            db.SaveChanges();
            
            return RedirectToAction("OrderList");
        }


        public ActionResult DeleteCart(int oId)
        {



            var orders = db.OrderList.FirstOrDefault(m => m.oId == oId);

            if (orders != null) 
            { 
                
            db.OrderList.Remove(orders);
            db.SaveChanges();
            }
       
            return RedirectToAction("OrderList");
        }
        public ActionResult UpdateQty(int oId,CartVM vm)
        {



            var orders = db.OrderList.FirstOrDefault(m => m.oId == oId);

            if (orders != null)
            {

                orders.oQty=vm.orderQty;
                db.SaveChanges();
            }

            return RedirectToAction("OrderList");
        }

        public ActionResult payment()
        {
            return View();
        }


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult payment(CartVM vm)
        //{

        //    var userId = (int)Session[CDictionary.SK_LOGINED_USER_ID];

        //    var user = db.Customer.FirstOrDefault(x => x.cId == userId);

        //    if (user.cPoint < vm.orderQty * vm.productPrice)
        //    {
        //        TempData[CDictionary.TK_Msg_SalesItem] = "點數餘額不足";
        //        return RedirectToAction("payment", new { pId = vm.pId }); //重整
        //    }
        //    else
        //    {
        //        user.cPoint -= vm.productPrice * vm.orderQty;
        //    }
        //    return View();

        //}

    }
}