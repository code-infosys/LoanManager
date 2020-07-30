using LoanManager.Models; 
using LoanManager.Repo;

namespace LoanManager.Service
{
    public interface IMenuBarService : IRepository<MenuPermission>
    {
        MenuPermission[] GetMenuBarlist(int RoleId,int UserId);
        MenuPermission[] GetMenuBarlist();
    }
}


