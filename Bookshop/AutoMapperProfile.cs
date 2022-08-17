using AutoMapper;
using Bookshop.DTOs.Product;
using Bookshop.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookshop
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ProductCreationDTO, Product>();
            CreateMap<FormatCreationDTO, Format>();
        }
    }
}
