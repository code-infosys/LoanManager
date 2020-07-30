using System;
using System.Collections.Generic; 
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
namespace LoanManager.Models
{
    public class Role : BaseEntity
    { 
       [Required]
       [DisplayName("Role Name")]
       [StringLength(50)] 
       public string RoleName { get; set; }

       [Required]
       [DisplayName("Is Active")]
       public bool IsActive { get; set; }

       public virtual ICollection<RoleUser> RoleUser_RoleIds { get; set; }

       public virtual ICollection<MenuPermission> MenuPermission_RoleIds { get; set; }


    }
}

