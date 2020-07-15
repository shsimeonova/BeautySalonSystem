using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BeautySalonSystem.UI.Pages
{
    public class LoginModel : PageModel
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private readonly IConfiguration _configuration;
        private readonly ILogger<IndexModel> _logger;

        public LoginModel(ILogger<IndexModel> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [BindProperty]
        public string Email { get; set; }
        [BindProperty, DataType(DataType.Password)]
        public string Password { get; set; }
        public string Message { get; set; }

        public async Task<IActionResult> OnGet(string returnUrl = "/Index")
        {
            // if (User.IsAuthenticated())
            // {
            //     if (returnUrl.Contains("handler"))
            //     {
            //         var pageName = returnUrl.Split("?handler=").First();
            //         var pageHandler = returnUrl.Splitll("?handler=").Last();
            //         return RedirectToPage(pageName, pageHandler);
            //     }
            //     
            //     return RedirectToPage(returnUrl);
            // }
            
            return new ChallengeResult("oidc", new AuthenticationProperties() { RedirectUri = returnUrl });
            // return new ChallengeResult("oidc");
        }
    }
}
