using LoanManager.Models;
using LoanManager.Repo;
namespace LoanManager.Service
{
    public interface IUserService : IRepository<User>
    {  
        DTResult<UserViewModel> GetGrid(DTParameters param,int user,int role);         
    }
}

