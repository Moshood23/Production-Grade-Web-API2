namespace Production_Grade_Web_API.Application.Interfaces
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
