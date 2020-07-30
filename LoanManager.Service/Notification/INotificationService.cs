using LoanManager.Models;
using LoanManager.Repo;
namespace LoanManager.Service
{
    public interface INotificationService : IRepository<Notification>
    {  
        DTResult<NotificationViewModel> GetGrid(DTParameters param);         
    }
}

