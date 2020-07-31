using System.Net.Http;
using BeautySalonSystem.UI.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using HttpMethod = Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http.HttpMethod;

namespace BeautySalonSystem.UI.Services
{
    public interface IUserService
    {
        UserPersonalInfo GetUserPersonalInfo(string userId);
    }
    
    public class UserService : MicroserviceHttpService, IUserService
    {
        public UserService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor) : base(httpClient, httpContextAccessor)
        {}
        
        public UserPersonalInfo GetUserPersonalInfo(string userId)
        {
            MicroserviceResponse response = Execute(
                $"{_client.BaseAddress}Users/{userId}",
                null,
                HttpMethod.Get);
            
            UserPersonalInfo result = JsonConvert.DeserializeObject<UserPersonalInfo>(response.ReturnData);

            return result;
        }
    }
}