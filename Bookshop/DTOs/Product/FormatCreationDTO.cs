using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookshop.DTOs.Product
{
    public class FormatCreationDTO
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public int BookId { get; set; }
        public bool IsDefault { get; set; } = false;
        public int Inventory { get; set; }
    }
}
