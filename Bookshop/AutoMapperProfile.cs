using AutoMapper;
using Bookshop.DTOs.Payment;
using Bookshop.DTOs.Product;
using Bookshop.DTOs.User;
using Bookshop.Entity;
using Bookshop.SQLContext;
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

            CreateMap<UserUpdateDTO, ApplicationUser>();
            CreateMap<Product, ProductGetDTO>();
            CreateMap<Genre, GenresFilterDTO>();
            CreateMap<ProductGenre, ProductGenresDTO>();
            CreateMap<AdminCreateDTO, Admin>();
            CreateMap<StaffCreateDTO, Staff>();
            CreateMap<UserCreateDTO, User>();
            CreateMap<PurchasedProductDTO, PurchasedProduct>();
            CreateMap<PurchasedHistoryDTO, PurchaseHistory>();
            CreateMap<CommentCreationDTO, Comment>();
        }
    }
}
