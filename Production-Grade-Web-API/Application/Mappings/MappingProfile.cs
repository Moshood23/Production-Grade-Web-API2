namespace Production.Grade.WebApi.Application.Mappings;

using System;
using AutoMapper;
using Production.Grade.WebApi.Application.Services;
using Production.Grade.WebApi.Domain.Entities;
using Production_Grade_Web_API.Application.DTO;
using Production_Grade_Web_API.Application.Validators;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateCategoryMaps();
        CreateProductMaps();
        CreatePictureMaps();
        CreateOrderMaps();
        CreateCartMaps();
        CreateUserMaps();
    }

    private void CreateCategoryMaps()
    {
        CreateMap<CreateCategoryDto, Category>();

        CreateMap<UpdateCategoryDto, Category>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<Category, CategoryResponseDto>()
            .ForMember(d => d.ProductCount, m => m.MapFrom(s => s.Products.Count));
    }

    private void CreateMap<T1, T2>()
    {
        throw new NotImplementedException();
    }

    private void CreateProductMaps()
    {
        CreateMap<CreateProductDto, Product>();

        CreateMap<UpdateProductDto, Product>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<Product, ProductResponseDto>()
            .ForMember(d => d.CategoryName, m => m.MapFrom(s => s.Category != null ? s.Category.Name : string.Empty))
            .ForMember(d => d.Pictures, m => m.MapFrom(s => s.Pictures));

        CreateMap<Product, ProductListDto>()
            .ForMember(d => d.CategoryName, m => m.MapFrom(s => s.Category != null ? s.Category.Name : string.Empty))
            .ForMember(d => d.ThumbnailUrl, m => m.MapFrom(s =>
                s.Pictures.OrderBy(p => p.DisplayOrder).FirstOrDefault() != null
                ? s.Pictures.OrderBy(p => p.DisplayOrder).First().Url
                : null));
    }

    private void CreatePictureMaps()
    {
        CreateMap<Picture, PictureResponseDto>();
    }

    private void CreateOrderMaps()
    {
        CreateMap<CreateOrderDto, Order>();

        CreateMap<CreateOrderItemDto, OrderItem>();

        CreateMap<OrderItem, OrderItemResponseDto>()
            .ForMember(d => d.ProductName, m => m.MapFrom(s => s.Product != null ? s.Product.Name : string.Empty))
            .ForMember(d => d.ProductSku, m => m.MapFrom(s => s.Product != null ? s.Product.SKU : string.Empty));

        CreateMap<Order, OrderResponseDto>()
            .ForMember(d => d.OrderItems, m => m.MapFrom(s => s.OrderItems));

        CreateMap<Order, OrderListDto>()
            .ForMember(d => d.ItemCount, m => m.MapFrom(s => s.OrderItems.Sum(oi => oi.Quantity)));
    }

    private void CreateCartMaps()
    {
        CreateMap<CreateCartItemDto, CartItem>();

        CreateMap<CartItem, CartItemResponseDto>()
            .ForMember(d => d.ProductName, m => m.MapFrom(s => s.Product != null ? s.Product.Name : string.Empty))
            .ForMember(d => d.ProductSku, m => m.MapFrom(s => s.Product != null ? s.Product.SKU : string.Empty));

        CreateMap<Cart, CartResponseDto>()
            .ForMember(d => d.Items, m => m.MapFrom(s => s.CartItems))
            .ForMember(d => d.TotalPrice, m => m.MapFrom(s => s.CalculatedTotalPrice))
            .ForMember(d => d.TotalItemCount, m => m.MapFrom(s => s.TotalItemCount));
    }

    private void CreateUserMaps()
    {
        CreateMap<ApplicationUser, UserProfileDto>();

        CreateMap<UpdateUserProfileDto, ApplicationUser>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}
