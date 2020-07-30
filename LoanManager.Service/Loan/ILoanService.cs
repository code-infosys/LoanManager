using LoanManager.Models;
using LoanManager.Repo;
namespace LoanManager.Service
{
    public interface ILoanService : IRepository<Loan>
    {  
        DTResult<LoanViewModel> GetGrid(DTParameters param);         
    }
}

