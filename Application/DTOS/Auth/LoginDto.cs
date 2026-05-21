using System.ComponentModel.DataAnnotations;

namespace Application.DTOS.Auth
{
    public record LogienDto([EmailAddress] string Email, string Password);

}
