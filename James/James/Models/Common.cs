using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace James.Models.Common
{
   
    public class page
    {
        public string controller { get; set; }
        public string action { get; set; }

        public object urlArgs { get; set; }
    }

    /// <summary>
    /// SESSION KEY OR ... 
    /// </summary>
    public class CDictionary
    {
        // SESSION KEY 
        public static readonly string SK_LOGINED_USER_ID = "SK_LOGINED_USER_ID";
        public static readonly string SK_LOGINED_ADMIN_ID = "SK_LOGINED_ADMIN_ID";
        public static readonly string SK_LOGINED_USER_AUTH = "SK_LOGINED_USER_AUTH";
        public static readonly string SK_PAGE_BEFORE_LOGIN = "SK_PAGE_BEFORE_LOGIN";
        public static readonly string SK_LOGINED_RESTAURANT_ID = "SK_LOGINED_RESTAURANT_ID";
        public static readonly string SK_MEMBER_AUTHORITY = "SK_MEMBER_AUTHORITY";


        // TEMP_DATA MESSAGE KEY
        public static readonly string TK_Msg_ChefCreate = "TK_Msg_ChefCreate";
        public static readonly int TK_Cart_Qty =0;
        public static readonly string TK_Msg_SaveCalendar = "TK_Msg_SaveCalendar";
        public static readonly string TK_Cart_TOTALPRICE = "TK_Cart_TOTALPRICE";

        public static readonly string DefaultImg = "/Content/img/noimg.jpg";
        public static readonly string picAdd_member = "Memberimage";
        public static readonly string picAdd_product = "Productimage";


        public static string[] Provience =
        {
            "Alberta",
            "British Columbia",
            "Manitoba",
            "New Brunswick",
            "Newfoundland and Labrador",
            "Northwest Territories",
            "Nova Scotia",
            "Nunavut",
            "Ontario",
            "Prince Edward Island",
            "Quebec",
            "Saskatchewan",
            "Yukon"


        };

    }
    public enum CAauthority
    {
        normalCustomer=1,
        admin=2,
        restaurant=3

    }

    public enum POfferdate
    {
        Enable=1,
        Disable=2
    }

    public enum orderStatus
    {
        beforeCheckOut=0,
        waitingForCheckOut=1,
        afterCheckOut=2,
        

    }

    public class 共用方法
    {
        public string 照片更新(HttpPostedFileBase httpPostedFile, string 根目錄, string 存檔位置, string 舊照片位置)
        {
            // 刪除舊照片
            if (System.IO.File.Exists($@"{根目錄 + 舊照片位置}"))
            {
                System.IO.File.Delete($@"{根目錄 + 舊照片位置}");
            }

            // 取得 . 的位置
            int point = httpPostedFile.FileName.IndexOf(".");
            // 取得 附檔名 *.jpg
            string estention = httpPostedFile.FileName
                .Substring(point, httpPostedFile.FileName.Length - point);
            // 不重複16位檔名
            string photoName = Guid.NewGuid().ToString() + estention;

            // 照片存檔
            string location = $@"/Content/{存檔位置}/" + photoName;
            httpPostedFile.SaveAs($@"{ 根目錄 + location }");

            // 回傳新路徑
            return location;
        }
    }
}