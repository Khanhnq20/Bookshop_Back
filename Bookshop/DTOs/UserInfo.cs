using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Bookshop.DTOs
{
    public class UserInfo 
    {
        [EmailAddress]
        public string Email { get; set; }
        public string UserName { get => this.Email; set => this.UserName = Email; }
        public string Password { get; set; }
        public string Name { get; set; }
        public DateTime DayOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }
        public string Role { get; set; }
    }
}
