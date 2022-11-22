using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookshop.DTOs.Product
{
    public class CommentCreationDTO
    {
        public string Content { get; set; }
        public int Rate { get; set; }
        public int ProductId { get; set; }
        public string Author { get; set; }
        public string EmailAth { get; set; }
    }
}
