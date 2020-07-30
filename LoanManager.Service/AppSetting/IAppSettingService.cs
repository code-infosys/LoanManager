using LoanManager.Models;
using LoanManager.Repo;
namespace LoanManager.Service
{
    public interface IAppSettingService : IRepository<AppSetting>
    {  
        DTResult<AppSettingViewModel> GetGrid(DTParameters param);         
    }
}

