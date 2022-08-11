using Bookshop.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookshop.Entity
{
    public class AuthResult : UserToken
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
    }
}
