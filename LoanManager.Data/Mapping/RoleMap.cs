using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LoanManager.Models;  
using Microsoft.EntityFrameworkCore;
namespace LoanManager.Data
{
    public class RoleMap
    {
        public RoleMap(EntityTypeBuilder<Role> tb)
        {
            tb.HasKey(o => o.Id);
            tb.Property(o => o.RoleName).HasMaxLength(50);

        } 
    }
}
