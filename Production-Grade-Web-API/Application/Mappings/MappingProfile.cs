namespace Production.Grade.WebApi.Application.Mappings;

using AutoMapper;
using Production.Grade.WebApi.Application.DTO;
using Production.Grade.WebApi.Application.DTOs;
using Production.Grade.WebApi.Domain.Entities;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // ============================
        // CATEGORY MAPPINGS
        // ============================
        CreateMap<CreateCategoryDto, Category>();

        CreateMap<Category, CategoryResponseDto>()
            .ForMember(
                dest => dest.ProductCount,
                opt => opt.MapFrom(src => src.Products.Count)
            );


        // ============================
        // PRODUCT MAPPINGS
        // ============================
        CreateMap<CreateProductDto, Product>();

        CreateMap<Product, ProductResponseDto>()
            .ForMember(
                dest => dest.CategoryName,
                opt => opt.MapFrom(src => src.Category == null ? "" : src.Category.Name)
            )
            .ForMember(
                dest => dest.Pictures,
                opt => opt.MapFrom(src => src.Pictures)
            );

        CreateMap<Product, ProductListDto>()
            .ForMember(
                dest => dest.CategoryName,
                opt => opt.MapFrom(src => src.Category == null ? "" : src.Category.Name)
            )
            .ForMember(
                dest => dest.ThumbnailUrl,
                opt => opt.MapFrom(src => GetFirstPictureUrl(src.Pictures))
            );


        // ============================
        // PICTURE MAPPINGS
        // ============================
        CreateMap<Picture, PictureResponseDto>();


        // ============================
        // ORDER MAPPINGS
        // ============================
        CreateMap<CreateOrderDto, Order>();
        CreateMap<CreateOrderItemDto, OrderItem>();

        CreateMap<OrderItem, OrderItemResponseDto>()
            .ForMember(
                dest => dest.ProductName,
                opt => opt.MapFrom(src => src.Product == null ? "" : src.Product.Name)
            )
            .ForMember(
                dest => dest.ProductSku,
                opt => opt.MapFrom(src => src.Product == null ? "" : src.Product.SKU)
            );

        CreateMap<Order, OrderResponseDto>()
            .ForMember(
                dest => dest.OrderItems,
                opt => opt.MapFrom(src => src.OrderItems)
            );

        CreateMap<Order, OrderListDto>()
            .ForMember(
                dest => dest.ItemCount,
                opt => opt.MapFrom(src => src.OrderItems.Sum(oi => oi.Quantity))
            );


        // ============================
        // CART MAPPINGS
        // ============================
        CreateMap<CreateCartItemDto, CartItem>();
        CreateMap<CartItem, CartItemResponseDto>()
            .ForMember(
                dest => dest.ProductName,
                opt => opt.MapFrom(src => src.Product == null ? "" : src.Product.Name)
            )
            .ForMember(
                dest => dest.ProductSku,
                opt => opt.MapFrom(src => src.Product == null ? "" : src.Product.SKU)
            );

        CreateMap<Cart, CartResponseDto>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.CartItems))
            .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.CalculatedTotalPrice))
            .ForMember(dest => dest.TotalItemCount, opt => opt.MapFrom(src => src.TotalItemCount));


        // ============================
        // USER MAPPINGS
        // ============================
        CreateMap<ApplicationUser, UserProfileDto>();
    }

    private static string? GetFirstPictureUrl(ICollection<Picture>? pictures)
    {
        if (pictures == null || pictures.Count == 0)
            return null;

        return pictures.OrderBy(p => p.DisplayOrder).FirstOrDefault()?.Url;
    }
}
