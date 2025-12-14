using AutoMapper;
using Production.Grade.WebApi.Domain.Entities;
using Production.Grade.WebApi.Application.DTO;
using Production.Grade.WebApi.Application.DTOs;

namespace Production_Grade_Web_API.Application.Interfaces
{
    public interface IMapper
    {
        T Map<T>(Production.Grade.WebApi.Application.Validators.UpdateUserProfileDto dto, ApplicationUser user);
        T Map<T>(ApplicationUser user);
        T Map<T>(Cart cart);
        T Map<T>(CartItem cartItem);
        T Map<T>(CreateCategoryDto dto);
        T Map<T>(Category category);
        T Map<T>(List<Category> userCategories);
        T Map<T>(Order order);
        T Map<T>(List<Order> userOrders);
        T Map<T>(CreateProductDto dto);
        T Map<T>(Product product);
        T Map<T>(List<Product> userProducts);
    }
}