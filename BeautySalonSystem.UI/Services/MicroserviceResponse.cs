using System.Net;

namespace BeautySalonSystem.UI.Services
{
    public class MicroserviceResponse
    {
        public string ReturnData { get; set; }
        public HttpStatusCode Code { get; set; }
    }
}