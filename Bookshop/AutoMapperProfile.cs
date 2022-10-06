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
            CreateMap<ProductCreationDTO, Product>()
                .ForMember(ent => ent.ProductGenres, dto => dto.MapFrom(p => p.GenreIds.Select(id => new ProductGenre() { GenreId = id })));
            CreateMap<ProductUpdateDTO,Product>()
                .ForMember(ent => ent.ProductGenres, dto => dto.MapFrom(p => p.GenreIds.Select(id => new ProductGenre() { GenreId = id }))); 
            CreateMap<FormatCreationDTO, Format>();
            CreateMap<GenreCreationDTO, Genre>();
        }
    }
}
