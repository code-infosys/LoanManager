using System;
using System.Collections.Generic; 
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
namespace LoanManager.Models
{
    public class GeneralSetting : BaseEntity
    { 
       [Required]
       [DisplayName("Setting Key")]
       [StringLength(50)] 
       public string SettingKey { get; set; }

       [Required]
       [DisplayName("Setting Value")]
       [StringLength(2000)] 
       public string SettingValue { get; set; }

       [DisplayName("Description")]
       [StringLength(100)] 
       public string Description { get; set; }

       [DisplayName("Setting Group")]
       [StringLength(50)] 
       public string SettingGroup { get; set; }


    }
}

