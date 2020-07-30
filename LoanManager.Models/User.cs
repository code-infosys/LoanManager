using System;
using System.Collections.Generic; 
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
namespace LoanManager.Models
{
    public class User : BaseEntity
    { 
       [Required]
       [DisplayName("User Name")]
       [StringLength(100)] 
       public string UserName { get; set; }

       [Required]
       [DisplayName("Password")]
       [StringLength(100)] 
       public string Password { get; set; }

       [Required]
       [DisplayName("Role")]
       public int RoleId { get; set; }

       [DisplayName("Date Added")]
       [Column(TypeName = "datetime")]
       public DateTime? DateAdded { get; set; }

       [DisplayName("Date Modified")]
       [Column(TypeName = "datetime")]
       public DateTime? DateModified { get; set; }

       [DisplayName("Full Name")]
       [StringLength(200)] 
       public string FullName { get; set; }

       [DisplayName("Email")]
       [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Please enter correct email")]
       [StringLength(50)] 

       public string Email { get; set; }

       [Required]
       [DisplayName("Mobile Number")]
       [StringLength(30)] 
       public string MobileNumber { get; set; }

       [Required]
       [DisplayName("Is Active")]
       public bool IsActive { get; set; }

       [DisplayName("Change Password Code")]
       [StringLength(100)] 
       public string ChangePasswordCode { get; set; }

       [Required]
       [DisplayName("Is Confirm")]
       public bool IsConfirm { get; set; }

       [Required]
       [DisplayName("On Time")]
       [StringLength(20)] 
       public string OnTime { get; set; }

       [Required]
       [DisplayName("Clock Test")]
       [StringLength(20)] 
       public string ClockTest { get; set; }

       [Required]
       [DisplayName("About")]
       [StringLength(500)] 
       public string About { get; set; }

       public virtual ICollection<RoleUser> RoleUser_UserIds { get; set; }

       public virtual ICollection<MenuPermission> MenuPermission_UserIds { get; set; }


    }
}

