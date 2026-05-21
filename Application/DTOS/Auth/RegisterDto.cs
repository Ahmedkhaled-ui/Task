using System.ComponentModel.DataAnnotations;

namespace Application.DTOS.Auth
{
    public record RegisterDto([EmailAddress] string Email, string DisplayName, string Password,
   string? UserName = "", string? PhoneNumber = "");
}
