using LoanManager.Models; 
using System.Linq;
using LoanManager.Repo;
using LoanManager.Data;

namespace LoanManager.Service
{
    public class MenuBarService : Repository<MenuPermission>, IMenuBarService
    {  
        public MenuBarService(ApplicationContext dbContext) : base(dbContext) { }

        public MenuPermission[] GetMenuBarlist(int RoleId, int UserId)
        {
            return GetAllInclude(i=>i.Menu_MenuId).Where(i => (i.RoleId == RoleId && i.UserId == null) || i.UserId == UserId).ToArray();
        }

        public MenuPermission[] GetMenuBarlist()
        {
            return GetAllInclude(i => i.Menu_MenuId).ToArray();
        }

        

        
    }
}


