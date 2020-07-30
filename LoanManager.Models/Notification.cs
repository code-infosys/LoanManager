using System;
using System.Collections.Generic; 
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
namespace LoanManager.Models
{
    public class Notification : BaseEntity
    { 
       [Required]
       [DisplayName("Table Name")]
       [StringLength(100)] 
       public string TableName { get; set; }

       [DisplayName("Table")]
       public int? TableId { get; set; }

       [Required]
       [DisplayName("Details")]
       [StringLength(300)] 
       public string Details { get; set; }

       [DisplayName("Process To Url")]
       [StringLength(400)] 
       public string ProcessToUrl { get; set; }

       [DisplayName("Is Read")]
       public bool? IsRead { get; set; }

       [DisplayName("Added By")]
       public int? AddedBy { get; set; }

       [DisplayName("Date Added")]
       [Column(TypeName = "datetime")]
       public DateTime? DateAdded { get; set; }

       [DisplayName("Modified By")]
       public int? ModifiedBy { get; set; }

       [DisplayName("Date Modified")]
       [Column(TypeName = "datetime")]
       public DateTime? DateModified { get; set; }

       [DisplayName("To User")]
       public int? ToUserId { get; set; }

       [DisplayName("To Role")]
       public int? ToRoleId { get; set; }

       [Required]
       [DisplayName("Unique")]
       [StringLength(50)] 
       public string UniqueId { get; set; }


    }
}

