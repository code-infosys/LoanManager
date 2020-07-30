using LoanManager.Models;
using LoanManager.Repo;
namespace LoanManager.Service
{
    public interface IRoleService : IRepository<Role>
    {  
        DTResult<RoleViewModel> GetGrid(DTParameters param);         
    }
}

