using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookshop.Entity
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Genre> Genres { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public DateTime PublishDay { get; set; }
        public string Pages { get; set; }
        public string Language { get; set; }
        public bool IsRemaining { get; set; }
        public List<Format> Type { get; set; }
    }
}
