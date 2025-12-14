using AutoMapper;
namespace Production.Grade.WebApi.Application.Interfaces
{
    public interface ISkuGenerationService
    {
        Task<string> GenerateSkuAsync();
    }

    public interface IOrderNumberGenerationService
    {
        Task<string> GenerateOrderNumberAsync();
    }

}
