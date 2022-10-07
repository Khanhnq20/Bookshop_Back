using Bookshop.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookshop.DTOs.Product
{
    public class ProductGetDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public DateTime PublishDay { get; set; }
        public int Pages { get; set; }
        public string Language { get; set; }
        public bool IsRemaining { get; set; }
        public string fileImage { get; set; }
        public List<ProductGenresDTO> ProductGenres { get; set; }
        public List<Format> Type { get; set; }
    }
}
