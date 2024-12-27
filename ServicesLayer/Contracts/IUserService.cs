using DataAccessLayer.Models;
using DataAccessLayer.Models.MyInMemoryApi.Models;
using DataAccessLayer.ViewModels;

namespace ServicesLayer.Contracts
{
    public interface IUserService
    {
        Task<ResultModel<User>> SignUpAsync(UserViewModel userVM);

        Task<ResultModel<User>> LoginAsync(UserViewModel userVM);
    }
}
