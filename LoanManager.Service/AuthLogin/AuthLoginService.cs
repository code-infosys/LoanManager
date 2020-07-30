using LoanManager.Models;
using LoanManager.Data;
using LoanManager.Repo;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LoanManager.Service
{
    public class AuthLoginService : Repository<User>, IAuthLoginService
    {
        public AuthLoginService(ApplicationContext dbContext) : base(dbContext) { }
        
        public Task<(bool, User)> ValidateUserCredentialsAsync(string username, string password)
        {
            var isValid = GetAll().FirstOrDefault(i => i.UserName == username && i.Password == password && i.IsActive == true);

            var result = (false, isValid);
            if (isValid != null)
                result = (true, isValid); 

            return Task.FromResult(result);
        }
        
        public Task<(bool, User)> ValidateUserCredentialsAsync(string username)
        {
            var isValid = GetAll().FirstOrDefault(i => i.UserName == username && i.IsActive == true);

            var result = (false, isValid);
            if (isValid != null)
                result = (true, isValid);

            return Task.FromResult(result);
        }

    }
}


