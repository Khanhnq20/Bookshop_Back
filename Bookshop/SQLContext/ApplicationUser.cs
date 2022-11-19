using Bookshop.DTOs;
using Bookshop.Entity;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Bookshop.SQLContext
{
    public class ApplicationUser: IdentityUser
    {
        public string Name { get; set; }
        public string DayOfBirth { get; set; }
        public string Gender { get; set; }
        public RefreshToken refreshToken { get; set; }
    }


    public class Admin : ApplicationUser
    {
    }
    public class Staff : ApplicationUser
    {
    }
    public class User : ApplicationUser
    {
        public List<PurchaseHistory> PurchaseHistories { get; set; }
    }
}