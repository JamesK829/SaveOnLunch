using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using James.Models;
using System.Data.Entity;

namespace James.ViewModels
{
    public class vRegisterVM
    {


        [Required(ErrorMessage = "Please Enter Your Email")]
        [StringLength(300, MinimumLength = 6, ErrorMessage = "Email Address Can't Be Longer Than 300 Words")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Enter Email Add")]
        public string Account { get; set; }


        [DataType(DataType.Password)]   //表示此欄位為密碼欄位，所以輸入時會產生隱碼
        [Required(ErrorMessage = "Please Enter Password！")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "6~20 words")]
        [RegularExpression(@"^(?=.*\d)(?=.*[A-Z])(?=.*[a-z]).{3,15}$", ErrorMessage = "密碼僅能有英文或數字，至少一個大小寫英+數！")]
        public string Password { get; set; }

        [DataType(DataType.Password)]   //表示此欄位為密碼欄位，所以輸入時會產生隱碼
        [Required(ErrorMessage = "請再次輸入密碼！")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "確認密碼的長度需再6~20個字元內！")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "兩次輸入的密碼必須相符！")]//跟System.Web.Mvc產生模稜兩可
        public string ConfirmPwd { get; set; }
        //[DataType(DataType.Text)]
        //[Required(ErrorMessage = "Please Enter Your Email")]
        //public string Account { get; set; }

        //[DataType(DataType.Password)]   //表示此欄位為密碼欄位，所以輸入時會產生隱碼
        //[Required(ErrorMessage = "Please Enter Your Password")]
        //public string Password { get; set; }

    }
}