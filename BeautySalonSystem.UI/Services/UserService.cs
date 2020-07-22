using BeautySalonSystem.UI.Models;
using IdentityModel.Client;

namespace BeautySalonSystem.UI.Services
{
    public interface IUserService
    {
        UserPersonalInfo GetUserPersonalInfo(string userId, string accessToken);
    }
    
    public class UserService : IUserService
    {
        public UserPersonalInfo GetUserPersonalInfo(string userId, string accessToken)
        {
            throw new System.NotImplementedException();
        }
    }
}