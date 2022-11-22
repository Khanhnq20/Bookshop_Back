using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Bookshop.Entity
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int Rate { get; set; }
        public string Author { get; set; }
        public string EmailAth { get; set; }

        [ForeignKey(nameof(product))]
        public int ProductId { get; set; }
        public Product product { get; set; }
    }
}
