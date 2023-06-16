using NewsTask.Mvc.Models;
using NewsTask.Mvc.RequestModels;

namespace NewsTask.Mvc.Interfaces
{
    public interface IAuthAPIManager
    {
        AuthViewModel Login(TokenRequestModel requestModel);
    }
}
