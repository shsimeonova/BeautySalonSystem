using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using BeautySalonSystem.UI.Util;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using HttpMethod = Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http.HttpMethod;

namespace BeautySalonSystem.UI.Services
{
    public class MicroserviceHttpService
    {
        protected readonly HttpClient _client;
        private IHttpContextAccessor _httpContextAccessor;
        
        protected MicroserviceHttpService(HttpClient client, IHttpContextAccessor httpContextAccessor)
        {
            _client = client;
            _httpContextAccessor = httpContextAccessor;
        }
        
        protected MicroserviceResponse Execute(string url, Object content, HttpMethod requestMethod)
        {
            string accessToken = _httpContextAccessor.HttpContext.GetTokenAsync("access_token").Result;
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            Task<HttpResponseMessage> requestTask = null;
            switch (requestMethod)
            {
                case HttpMethod.Get:
                    requestTask = _client.GetAsync(url);
                    break;
                case HttpMethod.Post:
                    requestTask = _client.PostAsync(url, new JsonContent(content));
                    break;
                case HttpMethod.Put:
                    requestTask = _client.PutAsync(url, new JsonContent(content));
                    break;
                case HttpMethod.Delete:
                    requestTask = _client.DeleteAsync(url);
                    break;
            }
            
            HttpResponseMessage httpResponseMessage = requestTask.Result;
            string responseBody = httpResponseMessage.Content.ReadAsStringAsync().Result;

            MicroserviceResponse result = new MicroserviceResponse {ReturnData = responseBody, Code = httpResponseMessage.StatusCode};

            return result;
        }
    }
}