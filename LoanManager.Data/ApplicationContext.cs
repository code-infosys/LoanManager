using Microsoft.EntityFrameworkCore;
using LoanManager.Models;

namespace LoanManager.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }
 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {  
            new RoleMap(modelBuilder.Entity<Role>());
            new UserMap(modelBuilder.Entity<User>());
            new RoleUserMap(modelBuilder.Entity<RoleUser>());
            new MenuMap(modelBuilder.Entity<Menu>());
            new MenuPermissionMap(modelBuilder.Entity<MenuPermission>());
            new AppSettingMap(modelBuilder.Entity<AppSetting>());
            new GeneralSettingMap(modelBuilder.Entity<GeneralSetting>());
            new NotificationMap(modelBuilder.Entity<Notification>());
            new LoanMap(modelBuilder.Entity<Loan>());
            new LoanManMap(modelBuilder.Entity<LoanMan>());

            base.OnModelCreating(modelBuilder);
        }
    }
}
