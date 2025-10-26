using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Abstraction;
using Shared.IdentityModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    public class AuthenticationController(IServiceManager _serviceManager) : ApiBaseController
    {
        // Login
        [HttpPost("Login")] // POST: baseUrl/Api/Authentication/Login
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _serviceManager.AuthenticationService.LoginAsync(loginDto);
            return Ok(user);
        }
        // Register
        [HttpPost("Register")] // POST: baseUrl/Api/Authentication/Register
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            var user = await _serviceManager.AuthenticationService.RegisterAsync(registerDto);
            return Ok(user);
        }

        // Check Email
        [HttpGet("CheckEmail")] // Get: baseUrl/Api/Authentication/CheckEmail
        public async Task<ActionResult<bool>> CheckEmail(string email)
        {
            var result = await _serviceManager.AuthenticationService.CheckEmailAsync(email);
            return Ok(result);
        }

        // Get Current User
        [Authorize]
        [HttpGet("CurrentUser")] // Get: baseUrl/Api/Authentication/CurrentUser
        public async Task<ActionResult<UserDto>> CurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var appUser = await _serviceManager.AuthenticationService.GetCurrentUserAsync(email!);

            return Ok(appUser);
        }
        // Get Current User Address
        [Authorize]
        [HttpGet("Address")] // Get: baseUrl/Api/Authentication/Address
        public async Task<ActionResult<AddressDto>> GetCurrentUserAddress()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var address = await _serviceManager.AuthenticationService.GetCurrentAddressAsync(email!);
            return Ok(address);
        }
        // Update Current User Address
        [Authorize]
        [HttpPut("Address")] // Put: baseUrl/Api/Authentication/Address
        public async Task<ActionResult<AddressDto>> UpdateCurrentUserAddress(AddressDto address)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var updatedAddress = await _serviceManager.AuthenticationService.UpdateCurrentUserAddressAsync(email!, address);
            return Ok(updatedAddress);
        }

    }
}
