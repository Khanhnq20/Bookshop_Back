using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Bookshop.Entity
{
    public class Genre
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [ForeignKey(nameof(Product))]
        public int? ProductId { get; set; }
        public Product Product { get; set; }
    }
}
