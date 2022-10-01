using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Bookshop.Entity
{
    public class Format
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public Product Book { get; set; }
        public bool IsDefault { get; set; }
        public int Inventory { get; set; }
    }
}
