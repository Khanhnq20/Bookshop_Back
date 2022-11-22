using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookshop.DTOs
{
    public class UserToken 
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime Expriration { get; set; }
    }
}
