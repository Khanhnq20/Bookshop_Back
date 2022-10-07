using Bookshop.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookshop.DTOs.Product
{
    public class GenresFilterDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ProductGenre> ProductGenres { get; set; }
    }
}
