using Bookshop.Entity;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookshop.DTOs.Product
{
    public class ProductGenresDTO
    {
        public int ProductId { get; set; }
        public int GenreId { get; set; }
        public GenresFilterDTO Genre { get; set; }
        public ProductGetDTO Product { get; set; }
    }
}
