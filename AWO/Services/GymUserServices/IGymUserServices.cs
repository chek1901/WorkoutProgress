using AWO.ViewModels.Account;
using AwoAppServices.Models;
using System.Threading.Tasks;

namespace AWO.Services.GymUserServices
{
    public interface IGymUserServices
    {
        Task<GymUsers> Get(int id);
        Task<UserExceptionMsg> Create(string firstName, string lastName, string email, string phone);
        Task Update(ManageAccountViewModel model);
    }
}
