using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LoanManager.Models;  
using Microsoft.EntityFrameworkCore;
namespace LoanManager.Data
{
    public class UserMap
    {
        public UserMap(EntityTypeBuilder<User> tb)
        {
            tb.HasKey(o => o.Id);
            tb.Property(o => o.UserName).HasMaxLength(100);
            tb.Property(o => o.Password).HasMaxLength(100);
            tb.Property(o => o.FullName).HasMaxLength(200);
            tb.Property(o => o.Email).HasMaxLength(50);
            tb.Property(o => o.MobileNumber).HasMaxLength(30);
            tb.Property(o => o.ChangePasswordCode).HasMaxLength(100);
            tb.Property(o => o.OnTime).HasMaxLength(20);
            tb.Property(o => o.ClockTest).HasMaxLength(20);
            tb.Property(o => o.About).HasMaxLength(500);

        } 
    }
}
