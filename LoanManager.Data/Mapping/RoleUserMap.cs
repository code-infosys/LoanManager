using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LoanManager.Models;  
using Microsoft.EntityFrameworkCore;
namespace LoanManager.Data
{
    public class RoleUserMap
    {
        public RoleUserMap(EntityTypeBuilder<RoleUser> tb)
        {
            tb.HasKey(o => o.Id);
            tb.HasOne(c => c.Role_RoleId).WithMany(o => o.RoleUser_RoleIds).HasForeignKey(o => o.RoleId).IsRequired(true).OnDelete(DeleteBehavior.Cascade);
            tb.HasOne(c => c.User_UserId).WithMany(o => o.RoleUser_UserIds).HasForeignKey(o => o.UserId).IsRequired(true).OnDelete(DeleteBehavior.Cascade);

        } 
    }
}
