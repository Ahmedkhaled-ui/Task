using Application.DTOS.Auth;
using Application.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services
{
   public class AuthServices(UserManager<IdentityUser> userManager, ITokenServices tokenServices) : IAuthService
    {
        public async Task<bool> CheckEmail(string email)
            => await userManager.FindByEmailAsync(email) is not null;

        public async Task<UserDto> Login(LogienDto loginDto)
        {
            var user = await userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
            {
                return new UserDto { Message = "Invalid email or password" };
            }

            var result = await userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!result)
            {
                return new UserDto { Message = "Invalid email or password" };
            }

            var roles = await userManager.GetRolesAsync(user);
            var token = tokenServices.GetToken(user.Id, user.UserName!, user.Email!, roles);

            return new UserDto 
            { 
                Email = user.Email!, 
                DisplayName = user.UserName!, 
                Token = token,
                IsAuthenticated = true
            };
        }

        public async Task<UserDto> Register(RegisterDto registerDto)
        {
            if (await CheckEmail(registerDto.Email))
            {
                return new UserDto { Message = "Email is already registered." };
            }

            var user = new IdentityUser
            {
                Email = registerDto.Email,
                NormalizedUserName = registerDto.DisplayName,
                UserName = registerDto.UserName ?? registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber
            };

            var result = await userManager.CreateAsync(user, registerDto.Password);
            if (result.Succeeded)
            {
                var token = tokenServices.GetToken(user.Id, user.UserName,user.Email, Array.Empty<string>());

                return new UserDto 
                { 
                    Email = user.Email!, 
                    DisplayName = user.UserName, 
                    Token = token,
                    IsAuthenticated = true
                };
            }

            var firstError = result.Errors.FirstOrDefault()?.Description ?? "Registration Failed";
            return new UserDto { Message = firstError };
        }
    }
    }
