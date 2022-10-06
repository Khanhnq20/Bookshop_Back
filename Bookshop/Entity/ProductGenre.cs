using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookshop.Entity
{
    public class ProductGenre
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int GenreId { get; set; }
        public Genre Genre { get; set; }
    }
}
