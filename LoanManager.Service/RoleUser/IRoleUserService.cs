using LoanManager.Models;
using LoanManager.Repo;
namespace LoanManager.Service
{
    public interface IRoleUserService : IRepository<RoleUser>
    {  
        DTResult<RoleUserViewModel> GetGrid(DTParameters param,int user,int role);         
    }
}

