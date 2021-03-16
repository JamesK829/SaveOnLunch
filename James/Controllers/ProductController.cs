using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using James.Models;
using James.ViewModels;
using PagedList;
using PagedList.Mvc;
using James.Models.Common;
using System.IO;

namespace James.Controllers
{
    public class ProductController : Controller
    {
        dbjEntities1 db = new dbjEntities1();

        private const int pageSize = 12;
       
        // GET: Product
        public ActionResult Index()
        {


            return View();
        }
        
      private IEnumerable<Style> styles
        {
            get { return db.Style.OrderBy(s => s.sId); }
        }
        public ActionResult ProductList(int page=1)
        {
            var query = db.Product.OrderBy(x => x.pId);

            int pageIndex = page < 1 ? 1 : page;

            var vm = new vProductListVM
            {

                searchParameter = new ProductSearchModel(),
                pageIndex = pageIndex,
                styleItems = new SelectList(this.styles, "sId", "style"),
                products = query.ToPagedList(pageIndex, pageSize)
            };

            return View(vm);
        }
        [HttpPost]
        public ActionResult ProductList(vProductListVM vm)
        {
            var query = db.Product.AsQueryable();
            if (!string.IsNullOrWhiteSpace(vm.searchParameter.productName))
            {
                query = query.Where(

                    x => x.pName.Contains(vm.searchParameter.productName));
            }
            int sId;
            if (!string.IsNullOrWhiteSpace(vm.searchParameter.style) && int.TryParse(vm.searchParameter.style, out sId))
            {
                query = query.Where(x => x.sId == sId);
            }
            query = query.OrderBy(x => x.pId);
            int pageIndex = vm.pageIndex < 1 ? 1 : vm.pageIndex;
            var resualt = new vProductListVM
            {
                //ordernum=(int)Session[CDictionary.TK_Cart_Qty],
                searchParameter = vm.searchParameter,
                pageIndex = vm.pageIndex < 1 ? 1 : vm.pageIndex,
                styleItems = new SelectList(styles, "sId", "style"),
                products = query.ToPagedList(pageIndex, pageSize)
            };
            return View(resualt);

        }

        public ActionResult ProductCreate()
        {
           


             var ListStyle = (new SelectionFactory()).SelectStyle();

            var vm = new vProductVM
            {
                
                Style = ListStyle,

            };
            return View(vm);
        }
        [HttpPost]
        public ActionResult ProductCreate(vProductVM vm)
        {
            if (ModelState.IsValid)
            {
                int userId = (int)Session[CDictionary.SK_LOGINED_USER_ID];

                var restaurant= db.Restaurant.Where(r => r.cId == userId).FirstOrDefault();
               
                Session[CDictionary.SK_LOGINED_RESTAURANT_ID] = restaurant.rId;

                if (vm.image != null)
                {
                    // 照片 新增 新菜品照片地址
                    vm.productImg = (new 共用方法()).照片更新(vm.image, Server.MapPath("~/"), CDictionary.picAdd_product, "");
                }

                
              
                db.Product.Add(new Product
                {
                    rId=restaurant.rId,
                    sId=vm.sId,
                    pName = vm.productName,
                    pPrice = vm.productPrice,
                    pInfo = vm.productContent,
                    pImage = vm.productImg,
                   pDate=vm.offerdate


                });
                db.SaveChanges();

                return RedirectToAction("ProductList");
            }

            var ListStyle = (new SelectionFactory()).SelectStyle();
            ListStyle.First(x => x.Value == vm.sId.ToString()).Selected=true;

            vm.Style = ListStyle;
            db.SaveChanges();
            return View(vm);
        }

        public ActionResult EditProduct(int pId)
        {


            var prod = db.Product.FirstOrDefault(p => p.pId == pId);

            //var timetable = (from o in db.OfferDate
            //                 where o.pId == prod.pId
            //                 select o).ToList();
            //var listUnable = new List<string>();
           
            //var listEnable = new List<string>();



            var ListStyle = (new SelectionFactory()).SelectStyle();
            ListStyle.First(x => x.Value == prod.sId.ToString()).Selected = true;

            var vm = new vProductVM
            {

                sId = (int)prod.sId,
                productName=prod.pName,
                productPrice=(decimal)prod.pPrice,
                productContent=prod.pInfo,
                productImg=prod.pImage,
                Style=ListStyle
                
            };
            

            return View(vm);

        }

        [HttpPost]
        public ActionResult EditProduct(vProductVM vm)
        {
            if (ModelState.IsValid)
            {
                
                var prod = db.Product.FirstOrDefault(p => p.pId == vm.pId);
                if (vm.image != null)
                {
                    // 照片 更新 
                    var 新菜品照片地址 = (new 共用方法()).照片更新(vm.image, Server.MapPath("~/"), CDictionary.picAdd_product, prod.pImage);
                    prod.pImage = 新菜品照片地址;
                }



                //db.SaveChanges();



                  prod.sId = vm.sId;
                prod.pName = vm.productName;
                prod.pPrice = vm.productPrice;
               // prod.pImage = vm.productImg;
                prod.pInfo = vm.productContent;

                
                db.SaveChanges();
               
                

                return RedirectToAction("ProductList");
            }

            var ListStyle = (new SelectionFactory()).SelectStyle();
            ListStyle.First(x => x.Value == vm.sId.ToString()).Selected = true;
            return View(vm);

        }

        public ActionResult ProductDelete(int pId)
        {
            var prod = db.Product.FirstOrDefault(p => p.pId == pId);
                if (prod != null)
            {
                db.Product.Remove(prod);
                db.SaveChanges();


            }
            return RedirectToAction("ProductList");
        }

        
        public ActionResult ProductDetail(int pId)
        {
         
            var userId = Session[CDictionary.SK_LOGINED_USER_ID];
           // Session.Remove(CDictionary.TK_Cart_Qty);

            var prod= db.Product.FirstOrDefault(p => p.pId == pId);

            var vm = (from p in db.Product
                      join s in db.Style on p.sId equals s.sId
                      where p.pId == prod.pId
                      select new vProductVM
                      {
                          cId= (int)userId,
                          pId = pId,
                          productName = p.pName,
                          productImg = p.pImage,
                          productPrice = (decimal)p.pPrice,
                          productContent = p.pInfo,
                          productstyle = s.style,
                       

                      }).FirstOrDefault();

           // Session[CDictionary.TK_Cart_Qty]=vm.ordernum;
         
            return View(vm);
        }
       [HttpPost]
        public ActionResult ProductDetail(vProductVM vm)
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


                orders.oQty = vm.ordernum;
                db.OrderList.Add(orders);

            }
            else
            {
                currentCart.oQty += vm.ordernum;
            }
            db.SaveChanges();




            return RedirectToAction("OrderList","Order");

        }

    }
}