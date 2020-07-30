using LoanManager.Models;
using LoanManager.Repo;
namespace LoanManager.Service
{
    public interface IMenuPermissionService : IRepository<MenuPermission>
    {  
        DTResult<MenuPermissionViewModel> GetGrid(DTParameters param,int user,int role);         
    }
}

