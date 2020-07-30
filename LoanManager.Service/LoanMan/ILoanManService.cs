using LoanManager.Models;
using LoanManager.Repo;
namespace LoanManager.Service
{
    public interface ILoanManService : IRepository<LoanMan>
    {  
        DTResult<LoanManViewModel> GetGrid(DTParameters param);         
    }
}

