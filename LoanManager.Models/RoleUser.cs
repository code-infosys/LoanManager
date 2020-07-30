using System;
using System.Collections.Generic; 
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
namespace LoanManager.Models
{
    public class RoleUser : BaseEntity
    { 
       [Required]
       [DisplayName("Role")]
       public int RoleId { get; set; }

       public virtual Role Role_RoleId { get; set; }

       [Required]
       [DisplayName("User")]
       public int UserId { get; set; }

       public virtual User User_UserId { get; set; }


    }
}

