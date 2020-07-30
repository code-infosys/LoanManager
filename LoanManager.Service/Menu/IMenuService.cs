using LoanManager.Models;
using LoanManager.Repo;
namespace LoanManager.Service
{
    public interface IMenuService : IRepository<Menu>
    {  
        DTResult<MenuViewModel> GetGrid(DTParameters param);         
    }
}

