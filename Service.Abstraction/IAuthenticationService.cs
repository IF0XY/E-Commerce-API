using Shared.IdentityModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Abstraction
{
    public interface IAuthenticationService
    {
        // Login
        // Take => Email, Password
        // Return Token, Email, Display Name
        Task<UserDto> LoginAsync(LoginDto loginDto);
        // Register
        // Take Email, Password, Display Name, UserName, Phone Number
        // Return Token, Email, Display Name
        Task<UserDto> RegisterAsync(RegisterDto registerDto);

        // Check Email
        // Take Email
        // Return bool
        Task<bool> CheckEmailAsync(string email);

        // Get Current User
        // Take Email
        // Return UserDto (Token, Email, Display Name)
        Task<UserDto> GetCurrentUserAsync(string email);

        // Get Current Address
        // Take Email 
        // Return AddressDto
        Task<AddressDto> GetCurrentAddressAsync(string email);
        // Update Current User Address
        // Take Email, Address
        // Return AddressDto
        Task<AddressDto> UpdateCurrentUserAddressAsync(string email, AddressDto addressDto);
    }
}
