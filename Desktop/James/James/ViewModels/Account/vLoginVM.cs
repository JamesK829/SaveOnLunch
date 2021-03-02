using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace James.ViewModels
{
    public class vLoginVM
    {
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please Enter Your Email")]
        public string Account { get; set; }

        [DataType(DataType.Password)]   //表示此欄位為密碼欄位，所以輸入時會產生隱碼
        [Required(ErrorMessage = "Please Enter Your Password")]
        public string Password { get; set; }


    }
}