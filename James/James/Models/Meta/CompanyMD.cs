
namespace James
{

    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    [MetadataType(typeof(CompanyMD))]
    public partial class Company
    {

        public class CompanyMD
        {

            [Key]
            [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
            public int CPId { get; set; }

            public int cityId { get; set; }

            [Display(Name = "Company Name")]
            [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
            public string CPName { get; set; }

            [Display(Name = "Company Branch")]
            [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
            public string CPBranch { get; set; }
            [Display(Name = "Company Add")]
            [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
            public string CPAdd { get; set; }

            [Display(Name = "Company Phone Number")]
            [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
            public string CPPhone { get; set; }
        }

    }
}