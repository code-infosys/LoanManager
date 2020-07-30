using LoanManager.Models; 
using System.Threading.Tasks;
using LoanManager.Repo;
namespace LoanManager.Service
{
    public interface IAuthLoginService : IRepository<User>
    { 
        Task<(bool, User)> ValidateUserCredentialsAsync(string username, string password);
        Task<(bool, User)> ValidateUserCredentialsAsync(string username);
    }
}


