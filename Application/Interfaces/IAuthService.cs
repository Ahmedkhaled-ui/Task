using Application.DTOS.Auth;

namespace Application.Interfaces
{
    public interface IAuthService
    {
        Task<UserDto> Login(LogienDto loginDto);
        Task<UserDto> Register(RegisterDto registerDto);
        Task<bool> CheckEmail(string email);
    }
}
