using Bookshop.DTOs;
using Microsoft.AspNetCore.Identity;

namespace Bookshop.SQLContext
{
    public class ApplicationUser: IdentityUser
    {
        public RefreshToken refreshToken { get; set; }
    }
}