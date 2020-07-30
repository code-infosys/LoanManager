using LoanManager.Models;
using LoanManager.Repo;
namespace LoanManager.Service
{
    public interface IGeneralSettingService : IRepository<GeneralSetting>
    {  
        DTResult<GeneralSettingViewModel> GetGrid(DTParameters param);         
    }
}

