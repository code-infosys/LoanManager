using System;
using System.Collections.Generic; 
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
namespace LoanManager.Models
{
    public class Loan : BaseEntity
    { 
       [Required]
       [DisplayName("Date Added")]
       [Column(TypeName = "datetime")]
       public DateTime DateAdded { get; set; }

       [Required]
       [DisplayName("Name")]
       [StringLength(100)] 
       public string Name { get; set; }


    }
}

