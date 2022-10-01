using Bookshop.Entity;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Bookshop.DTOs.Product
{
    public class ProductCreationDTO
    {
        public string Name { get; set; }
        public List<int> GenreIds { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public DateTime PublishDay { get; set; }
        public int Pages { get; set; }
        public string Language { get; set; }
        public bool IsRemaining { get; set; }
        public IFormFile fileImage { get; set; }
        public List<FormatCreationDTO> Type { get; set; }
    }
}
