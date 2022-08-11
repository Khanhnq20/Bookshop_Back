using Bookshop.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookshop.Service
{
    public class Responsitory
    {
        private List<Genre> _genres;
        public Responsitory()
        {
            _genres = new List<Genre>()
            {
                new Genre(){Id = 1, Name = "IT"},
                new Genre(){Id = 2, Name = "Maketing"}
            };
        }

        public List<Genre> GetAllGenre()
        {
            return _genres;
        }


             
    }
}
