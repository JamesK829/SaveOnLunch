using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;



namespace James.ViewModels
{
    public class vStorePointVM
    {
        public int cId { get; set; }

        [Display(Name ="Point")]
        [Required]
        [Range(0,100000)]
        public decimal point { get; set; }

    }
}