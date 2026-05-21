using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOS.Auth
{
    public class UserDto
    {
        public string Email { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public bool IsAuthenticated { get; set; } 
        public string Message { get; set; } = string.Empty; 


        public UserDto() { }

        public UserDto(string email, string displayName, string token)
        {
            Email = email;
            DisplayName = displayName;
            Token = token;
            IsAuthenticated = true;
        }
    }
}
