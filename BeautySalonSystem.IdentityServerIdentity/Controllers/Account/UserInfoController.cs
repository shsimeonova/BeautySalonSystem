using System;
using System.Linq;
using AutoMapper;
using BeautySalonSystem.Controllers;
using IdentityServerAspNetIdentity.Models;
using IdentityServerHost.Quickstart.UI.Models.Output;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServerAspNetIdentity.Controllers.Account
{
    public class UsersController : ApiController
    {
        private UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public UsersController(UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpGet("{userId}")]
        public IActionResult GetUserInfo(string userId)
        {
            var user = _userManager.FindByIdAsync(userId).Result;
            if (user == null)
            {
                return NotFound();
            }

            var result = _mapper.Map<ApplicationUser, UserPersonalInfoOutputModel>(user);

            var userClaims = _userManager.GetClaimsAsync(user).Result;
            result.FullName = userClaims.FirstOrDefault(c => c.Type.Equals("name"))?.Value;
            result.Role = userClaims.FirstOrDefault(c => c.Type.Equals("role"))?.Value;
            return Ok(result);
        }
    }
}